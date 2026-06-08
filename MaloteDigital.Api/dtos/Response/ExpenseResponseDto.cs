namespace MaloteDigital.Api.Dtos.Response;

public record ExpenseResponseDto(
    Guid Id,
    decimal Amount,
    DateTime DueDate,
    string Status,
    string DetailedDescription,
    string Type, 
    string? Observation
);
