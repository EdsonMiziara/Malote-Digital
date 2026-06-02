using FluentValidation;
using MaloteDigital.Api.dtos.Create;

namespace MaloteDigital.Api.Validators;

public class CondominiumValidator : AbstractValidator<CreateCondominiumDto>
{
    public CondominiumValidator()
    {
        RuleFor(x => x.PreferredPaymentDate)
            .NotEmpty()
            .InclusiveBetween(1, 31);
    }

}
