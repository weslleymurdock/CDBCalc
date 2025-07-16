using CDBCalculator.ApiGateway.UnitTests.Common.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SpaServices.StaticFiles;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CDBCalculator.ApiGateway.UnitTests.ProgramTests;

public class ProgramUnitTests
{
    [Theory]
    [InlineData("Docker", "wwwroot")]
    [InlineData("Production", "wwwroot")]
    [InlineData("Development", "cdbcalculator.client/dist/cdbcalculator.client")]
    public void SpaOptions_ShouldMatchExpectedRootPath(string environment, string expectedRoot)
    {
        var gateway = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(environment);
                builder.UseSolutionRelativeContentRoot("src/ApiGateway/CDBCalculator/CDBCalculator.Server");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Stub.json"), optional: false, reloadOnChange: false);
                });
                builder.UseStartup<Server.Startup>();
            });
        using var scope = gateway.Services.CreateScope();
        var provider = scope.ServiceProvider;

        var options = provider.GetService<IOptions<SpaStaticFilesOptions>>();
        Assert.NotNull(options);
        Assert.Equal(expectedRoot, options.Value.RootPath);
    }

}
