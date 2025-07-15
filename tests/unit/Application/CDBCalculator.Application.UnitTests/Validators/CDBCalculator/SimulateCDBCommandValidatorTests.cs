using Xunit;
using Application.CQRS.Requests.Commands;
using Application.Validators.CDBCalculator;

namespace CDBCalculator.Application.UnitTests.Validators.CDBCalculator;

public class SimulateCDBCommandValidatorTests
{
    private readonly SimulateCdbCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldThrow_WhenCommandIsNull()
    {
        Assert.Throws<InvalidOperationException>(() => _validator.Validate((SimulateCdbCommand)null!));
    }

    [Fact]
    public void Validate_ShouldFail_WhenMonthsAreZero()
    {
        var command = new SimulateCdbCommand { Months = 0, InitialValue = 1000 };
        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Months");
    }

    [Fact]
    public void Validate_ShouldFail_WhenInitialValueIsZero()
    {
        var command = new SimulateCdbCommand { Months = 12, InitialValue = 0.0 };
        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InitialValue");
    }

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new SimulateCdbCommand { Months = 6, InitialValue = 1000.0 };
        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }
}