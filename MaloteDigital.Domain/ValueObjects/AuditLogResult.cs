namespace MaloteDigital.Domain.ValueObjects;

public record AuditLogResult(
    string Action,
    string Details,
    string PerformedBy = "System/Mari"
    );
