using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application;

namespace Account.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountApplication(this IServiceCollection services)
        {
            // Add your dependency injection here.
            return services;
        }

        public static IApplicationBuilder UseAccountApplication(this IApplicationBuilder app)
        {
            app.UseHTASwagger();
            return app;
        }
    }
}