using CDBCalculator.ApiGateway.UnitTests.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CDBCalculator.ApiGateway.UnitTests.ProgramTests;

public class ProgramFunctionalTests
{
    [Theory]
    [InlineData("Development", "/swagger/index.html", true)]
    [InlineData("Development", "/index.html", true)]
    [InlineData("Development", "/alguma-rota-inexistente", true)] 
    [InlineData("Docker", "/index.html", true)]
    [InlineData("Production", "/swagger/index.html", true)]
    [InlineData("Production", "/index.html", true)]
    [InlineData("Production", "/rota-desconhecida", true)]         
    public async Task Endpoint_ShouldRespondByEnvironment(string env, string path, bool shouldSucceed)
    {
        var client = ApiGatewayTestFactory.CreateClient(env);
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
        var client = ApiGatewayTestFactory.CreateClient("Development");
        var response = await client.GetAsync("/swagger/index.html");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Swagger", html);
        Assert.Contains("CDB Calculator API", html);
    }

    [Fact]
    public async Task Fallback_ShouldServeIndexHtml()
    {
        var client = ApiGatewayTestFactory.CreateClient("Production");
        var response = await client.GetAsync("/maytheforcebewithyou");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("<html", html);
        Assert.Contains("<title>", html);
    }

    [Fact]
    public void SpaPath_ShouldBeWwwroot_WhenEnvironmentIsDocker()
    {
        var host = ApiGatewayTestFactory.CreateHost("Docker");
        var env = host.Services.GetRequiredService<IWebHostEnvironment>();
        var spaIndex = Path.Combine(env.ContentRootPath, "wwwroot", "index.html");
        Assert.True(File.Exists(spaIndex));
        Assert.Equal("Docker", env.EnvironmentName);
        Console.WriteLine($"Verificando SPA em: {spaIndex}");
        Assert.True(File.Exists(spaIndex), $"SPA index.html não encontrado em: {spaIndex}");
        // Adicional: checar se index.html existe em wwwroot
        Assert.True(File.Exists(Path.Combine(env.ContentRootPath, "wwwroot", "index.html")));
    }

}