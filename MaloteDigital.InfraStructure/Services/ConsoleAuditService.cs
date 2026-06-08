using MaloteDigital.Domain.Interfaces;
using MaloteDigital.Domain.ValueObjects;

namespace MaloteDigital.InfraStructure.Services;

public class ConsoleAuditService : IAuditService
{
    public Task LogAsync(AuditLogResult auditLog)
    {
        Console.WriteLine($"[AUDIT LOG] {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | Action: {auditLog.Action} | User: {auditLog.PerformedBy} | Details: {auditLog.Details}");

        return Task.CompletedTask;
    }
}
