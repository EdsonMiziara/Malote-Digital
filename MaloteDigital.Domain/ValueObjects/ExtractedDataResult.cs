namespace MaloteDigital.Domain.ValueObjects;

public record ExtractedDataResult(decimal Amount, DateTime? DueDate, DateTime? IssueDate);
