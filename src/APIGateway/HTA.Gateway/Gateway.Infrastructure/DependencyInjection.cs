using Microsoft.Extensions.DependencyInjection;
using Shared.Domain;
using Shared.Infrastructure;

namespace Gateway.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGatewayInfrastructure(this IServiceCollection services,
            CachingConfig cachingConfig)
        {
            services.AddSharedInfrastructure(cachingConfig: cachingConfig);
            return services;
        }
    }
}