namespace MaloteDigital.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public Guid CondominiumId { get; set; }
    public Condominium Condominium { get; set; }
    public string Beneficiary { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseEntryDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? RealPaymentDate { get; set; }
    public string Status { get; set; } = "Pendente";
    public string Observation { get; set; } = string.Empty;
    public string DetailedDescription { get; set; } = string.Empty;
}
