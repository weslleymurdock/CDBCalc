using Application.CQRS.Requests.Commands;
using FluentValidation;

namespace Application.Validators.CDBCalculator;

public class SimulateCDBCommandValidator : AbstractValidator<SimulateCDBCommand>
{
    public SimulateCDBCommandValidator()
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
