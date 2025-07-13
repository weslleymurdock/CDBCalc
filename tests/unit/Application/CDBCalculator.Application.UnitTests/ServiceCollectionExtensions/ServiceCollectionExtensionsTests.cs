using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application;
using Infrastructure.Common.Interfaces;
using FluentValidation;
using MediatR;
using SimulateCDBCommand = Application.CQRS.Requests.Commands.SimulateCDBCommand;
namespace CDBCalculator.Application.UnitTests.ServiceCollectionExtensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddApplication_ShouldRegisterDependencies()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder().Build();

        services.AddApplication(config);
        var provider = services.BuildServiceProvider();

        var mediator = provider.GetService<IMediator>();
        var validator = provider.GetService<IValidator<SimulateCDBCommand>>();
        var calc = provider.GetService<ICDBCalculator>();

        Assert.NotNull(mediator);
        Assert.NotNull(validator);
        Assert.NotNull(calc);
    }
}