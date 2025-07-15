using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace CDBCalculator.ApiGateway.UnitTests.Common.Providers;

public class TestProxyConfigProvider : IProxyConfigProvider
{
    private readonly InMemoryConfig _config;

    public TestProxyConfigProvider(string routePathPrefix, string clusterId, string destinationName, string destinationAddress)
    {
        _config = new InMemoryConfig(
        [
            new RouteConfig
            {
                RouteId = $"{clusterId}-Route",
                ClusterId = clusterId,
                Match = new RouteMatch
                {
                    Path = $"{routePathPrefix}/{{**catch-all}}"
                },
                Transforms =
                [
                    new Dictionary<string, string>
                    {
                        { "PathRemovePrefix", routePathPrefix }
                    }
                ]
            }
        ],
        [
            new ClusterConfig
            {
                ClusterId = clusterId,
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    {
                        destinationName, new DestinationConfig
                        {
                            Address = destinationAddress
                        }
                    }
                }
            }
        ]);
    }

    public IProxyConfig GetConfig() => _config;

    private class InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters) : IProxyConfig
    {
        public IReadOnlyList<RouteConfig> Routes { get; } = routes;
        public IReadOnlyList<ClusterConfig> Clusters { get; } = clusters;
        public IChangeToken ChangeToken { get; } = new CancellationChangeToken(new CancellationTokenSource().Token);
    }
}

