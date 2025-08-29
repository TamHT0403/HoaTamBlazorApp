using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Shared.Domain;

namespace Shared.Application
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSharedApplication(this IServiceCollection services)
        {
            // Add your dependency injection here.
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSharedApplication(this IApplicationBuilder app)
        {

            return app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHTASwagger(this IServiceCollection services, OpenApiInfo? openApiInfo = null,
            string? description = null)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", openApiInfo ?? new OpenApiInfo
                {
                    Title = "HTA API",
                    Version = "v1",
                    Description = "API documentation for HTA microservices"
                });

                // 🔑 Thêm cấu hình JWT Bearer Authentication
                c.AddSecurityDefinition(name: JWTConstant.AUTH_JWT_SCHEME, securityScheme: new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = description ?? "Nhập JWT token theo format: Bearer {token}",
                    Name = JWTConstant.AUTH_JWT_NAME,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JWTConstant.AUTH_JWT_SCHEME,
                    BearerFormat = JWTConstant.AUTH_JWT_FORMAT
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JWTConstant.AUTH_JWT_SCHEME
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // 🔧 Thêm các header custom (ví dụ: x-tenant-id, x-request-id)
                c.OperationFilter<AddRequiredHeadersOperationFilter>();
            });

            return services;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHTASwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
