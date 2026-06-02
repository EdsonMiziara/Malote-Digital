namespace MaloteDigital.Api.dtos.Create;

public record CreateExpenseDto(
    Guid CondominiumId,
    string Description,
    decimal Amount,
    DateTime DueDate
);
