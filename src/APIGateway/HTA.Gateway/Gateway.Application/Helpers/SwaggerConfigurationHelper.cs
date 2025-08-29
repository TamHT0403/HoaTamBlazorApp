using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Gateway.Application
{
    public class SwaggerConfigurationHelper
    {
        public static void ConfigureWithAuth(
        IServiceCollection services,
        string authority,
        Dictionary<string, string> scopes,
        string apiTitle,
        string apiVersion = "v1"
    )
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(apiVersion, new OpenApiInfo { Title = apiTitle, Version = apiVersion });

                // OAuth2 + JWT
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                            TokenUrl = new Uri($"{authority}/connect/token"),
                            Scopes = scopes
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    scopes.Keys.ToList()
                }
                });

                options.CustomSchemaIds(type => type.FullName);
            });
        }
    }
}