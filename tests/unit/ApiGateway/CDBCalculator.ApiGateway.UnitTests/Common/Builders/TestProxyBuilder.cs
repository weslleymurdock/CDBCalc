using CDBCalculator.ApiGateway.UnitTests.Common.Handlers;
using CDBCalculator.ApiGateway.UnitTests.Common.Providers;
using CDBCalculator.ApiGateway.UnitTests.Common.Servers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;
namespace CDBCalculator.ApiGateway.UnitTests.Common.Builders;

public static class TestProxyBuilder
{
    public static (HttpClient GatewayClient, MicroServiceTestServer MicroService) Create(string env = "Development")
    {
        var contentRoot = Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "../../../../src/ApiGateway/CDBCalculator/CDBCalculator.Server"));

        var microService = new MicroServiceTestServer();

        var gateway = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(env);
                builder.UseContentRoot(contentRoot);
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Stub.json"), optional: false, reloadOnChange: false);
                });
                if (env.Equals("Test", StringComparison.OrdinalIgnoreCase))
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IProxyConfigProvider>(
                            new TestProxyConfigProvider(
                                routePathPrefix: "/gateway",
                                clusterId: "cdbCluster",
                                destinationName: "default",
                                destinationAddress: microService.Client.BaseAddress!.ToString()
                            )
                        );
                        services.AddSingleton(new HttpMessageInvoker(
                            new CdbCalculatorMicroServiceHandler(microService.Client))
                        );
                    });
                }
            });


        var client = gateway.CreateClient();

        return (client, microService);
    }
}
