using System.Security.Claims;
using System.Threading.RateLimiting;
using Gateway.Application;
using Gateway.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Shared.Domain;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

SwaggerConfigurationHelper.ConfigureWithAuth(
    builder.Services,
    authority: builder.Configuration["AuthServer:Authority"]!,
    scopes: new Dictionary<string, string>
    {
        { "Account.API", "Account Service" },
        { "Product.API", "Product Service" }
    },
    apiTitle: "Gateway APIs"
);

// 5) Rate limiting (per-user + per-IP). In-memory per instance.
// Với multi-instance production, chuyển sang Redis-based counter (custom) để đồng bộ.
builder.Services.AddRateLimiter(opts =>
{
    opts.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    var userCfg = builder.Configuration.GetSection("RateLimiting:PerUser");
    var ipCfg = builder.Configuration.GetSection("RateLimiting:PerIp");

    opts.AddPolicy("PerUser", httpContext =>
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        // nếu chưa đăng nhập → rate-limit theo IP
        var partitionKey = string.IsNullOrWhiteSpace(userId)
            ? httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip"
            : $"user:{userId}";

        return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = userCfg.GetValue("PermitLimit", 300),
            Window = TimeSpan.FromSeconds(userCfg.GetValue("WindowSeconds", 60)),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = userCfg.GetValue("QueueLimit", 0)
        });
    });

    // Optional: policy riêng cho IP nếu muốn áp độc lập
    opts.AddPolicy("PerIp", httpContext =>
    {
        var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip";
        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = ipCfg.GetValue("PermitLimit", 200),
            Window = TimeSpan.FromSeconds(ipCfg.GetValue("WindowSeconds", 60)),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = ipCfg.GetValue("QueueLimit", 0)
        });
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {
        // Bảo đảm forward header Authorization + user context nếu cần
        builderContext.AddRequestTransform(async transformContext =>
        {
            // giữ nguyên Authorization header
            // thêm x-correlation-id nếu chưa có
            if (!transformContext.HttpContext.Request.Headers.ContainsKey("x-correlation-id"))
            {
                transformContext.ProxyRequest.Headers.TryAddWithoutValidation("x-correlation-id",
                    transformContext.HttpContext.TraceIdentifier);
            }
            await Task.CompletedTask;
        });
    });

builder.Services.AddGatewayApplication(configuration: builder.Configuration);

builder.Services.AddGatewayInfrastructure(cachingConfig: new CachingConfig()
{
    RedisConfig = new RedisConfig()
    {
        ConnectionString = builder.Configuration["Caching:Redis:ConnectionString"] ?? string.Empty,
        InstanceName = builder.Configuration["Caching:Redis:InstanceName"] ?? string.Empty
    }
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCors(cor =>
{
    cor.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .WithExposedHeaders();
});

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.UseSwaggerUIWithYarp(app.Configuration);

app.UseRewriter(new RewriteOptions().AddRedirect("^(|\\|\\s+)$", "/swagger"));

app.MapReverseProxy();

app.Run();