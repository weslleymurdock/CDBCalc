using CDBCalculator.ApiGateway.UnitTests.Common.Builders;
using CDBCalculator.ApiGateway.UnitTests.Common.Handlers;
using CDBCalculator.ApiGateway.UnitTests.Common.Servers;
using CDBCalculator.ApiGateway.UnitTests.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CDBCalculator.ApiGateway.UnitTests.ProgramTests;

public class ProgramFunctionalTests
{
    [Theory]
    [InlineData("Development", "/swagger/index.html", true)]
    [InlineData("Development", "/rota-desconhecida", false)]
    [InlineData("Docker", "/swagger/index.html", true)]
    [InlineData("Docker", "/rota-desconhecida", false)]
    [InlineData("Production", "/swagger/index.html", true)]
    [InlineData("Production", "/rota-desconhecida", false)]         
    public async Task Endpoint_ShouldRespondByEnvironment(string env, string path, bool shouldSucceed)
    {
        var (client, _) = TestProxyBuilder.Create(env);
        var response = await client.GetAsync(path);

        var body = await response.Content.ReadAsStringAsync();

        if (shouldSucceed)
        {
            response.EnsureSuccessStatusCode();
            Assert.False(string.IsNullOrWhiteSpace(body));
        }
        else
        {
            Assert.False(response.IsSuccessStatusCode);
        }
    }

    [Fact]
    public async Task Swagger_ShouldExposeMetadata()
    {
        var (gatewayClient, microService) = TestProxyBuilder.Create();

        using (microService)
        {
            var response = await gatewayClient.GetAsync("/swagger/v1/swagger.json");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("Simulador de Cdb", json);
        }
    }

    [Fact]
    public void SpaPath_ShouldBeWwwroot_WhenEnvironmentIsDocker()
    {
        var host = HostFactory.CreateHost("Docker");
        var env = host.Services.GetRequiredService<IWebHostEnvironment>();
        var spaIndex = Path.Combine(env.ContentRootPath, "wwwroot", "index.html");
        Assert.True(File.Exists(spaIndex));
        Assert.Equal("Docker", env.EnvironmentName);
        Assert.True(File.Exists(Path.Combine(env.ContentRootPath, "wwwroot", "index.html")));
    }

}