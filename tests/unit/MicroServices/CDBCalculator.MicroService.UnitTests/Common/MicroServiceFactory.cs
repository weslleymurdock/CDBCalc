using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace CDBCalculator.MicroService.UnitTests.Common;

public static class MicroServiceFactory
{
    public static HttpClient CreateClient(string environment = "Development")
    {
        var contentRoot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..", "..", "..",
            "src", "MicroServices", "CDBCalculator"
        ));

        var builder = new WebHostBuilder()
            .UseEnvironment(environment)
            .UseContentRoot(contentRoot)
            .UseStartup<Startup>();

        var server = new TestServer(builder);
        return server.CreateClient();
    }
    public static IHost CreateHost(string environment = "Development")
    {
        return Program.CreateHostBuilder([$"--environment={environment}"])
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer();
            })
            .Start();
    }

}
