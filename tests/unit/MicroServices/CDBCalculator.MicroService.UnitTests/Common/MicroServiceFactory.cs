using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace CDBCalculator.MicroService.UnitTests.Common;

public static class MicroServiceFactory
{
    public static HttpClient CreateClient(string environment = "Development")
    {
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(environment);
                builder.UseSolutionRelativeContentRoot("src/ApiGateway/CDBCalculator/CDBCalculator.Server");
            });

        return factory.CreateClient();
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
