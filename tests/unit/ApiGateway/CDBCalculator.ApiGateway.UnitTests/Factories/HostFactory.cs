using CDBCalculator.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace CDBCalculator.ApiGateway.UnitTests.Factories;

public static class HostFactory
{
    public static IHost CreateHost(string environment = "Development")
    {
        return Program.CreateHostBuilder([$"--environment={environment}"])
            .ConfigureWebHost(builder =>
            {
                builder.UseTestServer();
            })
            .Start();
    }

}
