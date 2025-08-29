using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Shared.Domain;
using Shared.Application;

namespace Gateway.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGatewayApplication(this IServiceCollection services,
            IConfiguration configuration)
        {
            var endpoints = new List<string>()
            {
                configuration["Endpoints:Gateway"] ?? string.Empty,
                configuration["Endpoints:Account"] ?? string.Empty,
                configuration["Endpoints:Product"] ?? string.Empty
            };
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(configureOptions: options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuers = endpoints,
                        ValidAudiences = endpoints,
                        IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: configuration["JwtConfig:SecretKey"] ?? string.Empty))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            ILogger<JwtBearerEvents> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                            logger.LogError(context.Exception, "Authentication failed.");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            ILogger<JwtBearerEvents> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                            logger.LogInformation("Token validated successfully.");
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            ILogger<JwtBearerEvents> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                            logger.LogInformation("Message received.");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            DateTime timestamp = DateTime.UtcNow;
                            string traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
                            ErrorDetail errorDetail = new ErrorDetail(
                                Code: "Unauthorized",
                                Message: context.Error ?? "Authentication failed."
                            );
                            ResponseResult<object> response = new ResponseResult<object>(
                                Success: false,
                                Data: null,
                                Error: errorDetail,
                                Timestamp: timestamp,
                                TraceId: traceId
                            );
                            return context.Response.WriteAsync(text: response.ToJson());
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            DateTime timestamp = DateTime.UtcNow;
                            string traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
                            ErrorDetail errorDetail = new ErrorDetail(
                                Code: "Forbidden",
                                Message: "Forbidden access."
                            );
                            ResponseResult<object> response = new ResponseResult<object>(
                                Success: false,
                                Data: null,
                                Error: errorDetail,
                                Timestamp: timestamp,
                                TraceId: traceId
                            );
                            return context.Response.WriteAsync(text: response.ToJson());
                        }
                    };
                });
            return services;
        }
    }
}