using FluentValidation;
using MaloteDigital.Api.dtos.Update;
using System.Net.NetworkInformation;

namespace MaloteDigital.Api.Validators;

public class UpdateExpenseStatusValidator : AbstractValidator<UpdateExpenseStatusDto>
{
    public UpdateExpenseStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("O status não pode ser vazio.")
            .Must(status => status == "Pendente" || status == "Pago")
            .WithMessage("O status deve ser 'Pendente' ou 'Pago'.");
    }
}
