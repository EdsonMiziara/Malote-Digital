namespace MaloteDigital.Domain.ValueObjects;

public record class OfxTransactionResult(string TransactionId, decimal Amount, DateTime DatePosted, string Description);
