using MaloteDigital.Domain.ValueObjects;

namespace MaloteDigital.Domain.Interfaces;

public interface IAuditService
{
    Task LogAsync(AuditLogResult auditLogResult);

}
