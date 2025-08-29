using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Application
{
    public static class YarpSwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerUIWithYarp(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("SwaggerUIWithYarp");

                var proxyConfigProvider = app.ApplicationServices.GetRequiredService<IProxyConfigProvider>();
                var yarpConfig = proxyConfigProvider.GetConfig();

                var routedClusters = yarpConfig.Clusters
                    .SelectMany(cluster => cluster.Destinations ?? new Dictionary<string, DestinationConfig>(),
                        (cluster, destination) => new { cluster.ClusterId, destination.Value });

                var groupedClusters = routedClusters
                    .GroupBy(x => x.Value.Address)
                    .Select(g => g.First())
                    .ToList();

                foreach (var clusterGroup in groupedClusters)
                {
                    var routeConfig = yarpConfig.Routes.FirstOrDefault(r => r.ClusterId == clusterGroup.ClusterId);
                    if (routeConfig == null)
                    {
                        logger.LogWarning($"Swagger UI: Couldn't find route configuration for {clusterGroup.ClusterId}...");
                        continue;
                    }

                    // Map swagger.json từ service con
                    options.SwaggerEndpoint($"{clusterGroup.Value.Address}/swagger/v1/swagger.json", $"{routeConfig.RouteId} API");
                }

                // OAuth2
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            });

            return app;
        }
    }
}