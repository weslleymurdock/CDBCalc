using Infrastructure;
using Infrastructure.Common.Interfaces;
using InfraCalc = Infrastructure.Services.CDBCalculator.CDBCalculator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CDBCalculator.Infrastructure.UnitTests.ServicesCollections;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddInfrastructure_ShouldRegisterICDBCalculator()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddInfrastructure(configuration);
        var provider = services.BuildServiceProvider();

        var instance = provider.GetService<ICDBCalculator>();

        Assert.NotNull(instance);
        Assert.IsType<InfraCalc>(instance);
    }
}