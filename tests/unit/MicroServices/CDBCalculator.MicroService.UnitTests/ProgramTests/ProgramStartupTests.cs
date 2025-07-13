using CDBCalculator.MicroService.UnitTests.Common;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
namespace CDBCalculator.MicroService.UnitTests.ProgramTests;

public class ProgramStartupTests
{

    [Fact]
    public void ShouldRegisterCorsPolicy()
    {
        var host = MicroServiceFactory.CreateHost("Development");
        var corsOptions = host.Services.GetRequiredService<IOptions<CorsOptions>>();

        var policyExists = corsOptions.Value.GetPolicy("AllowGateway") != null;
        Assert.True(policyExists);
    }

    [Fact]
    public void Environment_ShouldAffectConfiguration()
    {
        var host = MicroServiceFactory.CreateHost("Production");
        Assert.True(host.Services.GetRequiredService<IHostEnvironment>().IsProduction());
    }
}
