using FluentValidation;
using MaloteDigital.Domain.Entities;

namespace MaloteDigital.Api.Validators;

public class CondominiumValidator : AbstractValidator<Condominium>
{
    public CondominiumValidator()
    {
        RuleFor(x => x.PreferredPaymentDate)
            .NotEmpty()
            .InclusiveBetween(1, 31);
    }

}
