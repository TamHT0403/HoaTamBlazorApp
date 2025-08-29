using Microsoft.Extensions.DependencyInjection;
using Shared.Application;
using Shared.Domain;

namespace Shared.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services,
            CachingConfig cachingConfig)
        {
            services.AddStackExchangeRedisCache(setupAction: (cache) =>
            {
                cache.Configuration = cachingConfig.RedisConfig.ConnectionString;
                cache.InstanceName = cachingConfig.RedisConfig.InstanceName;
            });

            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
