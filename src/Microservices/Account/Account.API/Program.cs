using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Shared.Application;
using Shared.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHTASwagger(openApiInfo: new OpenApiInfo()
{
    Title = "Account API",
    Description = "API quản lý tài khoản và quyền truy cập cho hệ thống Hoa Tâm",
    Version = "v1.0",
    TermsOfService = new Uri("https://hoatam.vn/terms"),
    Contact = new OpenApiContact()
    {
        Name = SystemConstant.COMPANY_NAME,
        Email = SystemConstant.COMPANY_EMAIL,
        Url = new Uri(SystemConstant.COMPANY_URL),
        Extensions = new Dictionary<string, IOpenApiExtension>
        {
            { "x-phone", new Microsoft.OpenApi.Any.OpenApiString(SystemConstant.COMPANY_PHONE) },
            { "x-address", new Microsoft.OpenApi.Any.OpenApiString(SystemConstant.COMPANY_ADDRESS) }
        }
    },
    License = new OpenApiLicense()
    {
        Name = "MIT License",
        Url = new Uri("https://opensource.org/licenses/MIT")
    }
});


var app = builder.Build();

app.UseCors(cor =>
{
    cor.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});

app.UseHTASwagger();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();