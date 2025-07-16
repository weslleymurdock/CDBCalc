using Application.CQRS.Requests.Commands;
using FluentValidation;

namespace Application.Validators.CDBCalculator;

/// <summary>
/// Provides validation rules for the <see cref="SimulateCdbCommand"/> object.
/// </summary>
/// <remarks>This validator ensures that the <see cref="SimulateCdbCommand"/> object and its properties meet the
/// required conditions. It checks for null or empty values and enforces minimum constraints on numeric
/// properties.</remarks>
public class SimulateCdbCommandValidator : AbstractValidator<SimulateCdbCommand>
{
    /// <summary>
    /// Validates the properties of a SimulateCdbCommand object to ensure they meet the required criteria.
    /// </summary>
    /// <remarks>This validator enforces the following rules: <list type="bullet"> <item><description>The
    /// command object must not be null or empty.</description></item> <item><description>The <c>Months</c> property
    /// must not be null or empty and must be greater than or equal to 1.</description></item> <item><description>The
    /// <c>InitialValue</c> property must not be null or empty and must be greater than or equal to
    /// 1.00.</description></item> </list> Use this validator to ensure that a SimulateCdbCommand object is properly
    /// configured before processing.</remarks>
    public SimulateCdbCommandValidator()
    {
        RuleFor(cdb => cdb)
            .NotNull()
            .WithMessage("The command object must not be null")
            .NotEmpty()
            .WithMessage("The command object must not be empty");
        RuleFor(cdb => cdb.Months)
            .NotEmpty()
            .WithMessage("The months command poperty must not be empty")
            .NotNull()
            .WithMessage("The months command poperty must not be null");
        RuleFor(cdb => cdb.Months)
            .GreaterThanOrEqualTo(1u).WithMessage("The months property value must be greather than or equals to 1");
        RuleFor(cdb => cdb.InitialValue)
            .NotEmpty()
            .WithMessage("The months command poperty must not be empty")
            .NotNull()
            .WithMessage("The months command poperty must not be null");
        RuleFor(cdb => cdb.InitialValue)
            .GreaterThanOrEqualTo(1.00d).WithMessage("The months property value must be greather than or equals to 1.00");

    }
}
