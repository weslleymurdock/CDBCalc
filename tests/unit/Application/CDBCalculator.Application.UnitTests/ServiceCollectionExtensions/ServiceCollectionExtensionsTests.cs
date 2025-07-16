using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application;
using Infrastructure.Common.Interfaces;
using FluentValidation;
using MediatR;
using SimulateCdbCommand = Application.CQRS.Requests.Commands.SimulateCdbCommand;
namespace CDBCalculator.Application.UnitTests.ServiceCollectionExtensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddApplication_ShouldRegisterDependencies()
    {
        var services = new ServiceCollection();

        services.AddApplication();
        var provider = services.BuildServiceProvider();

        var mediator = provider.GetService<IMediator>();
        var validator = provider.GetService<IValidator<SimulateCdbCommand>>();
        var calc = provider.GetService<ICdbCalculator>();

        Assert.NotNull(mediator);
        Assert.NotNull(validator);
        Assert.NotNull(calc);
    }
}