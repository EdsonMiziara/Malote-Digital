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


    public void CalculatePreferredDate(int PreferredDay)
    {
        if(PreferredDay == 0) 
        {
            RealPaymentDate = DueDate;
            return; 
        }

        try
        {
            var targetDate = new DateTime(DueDate.Year, DueDate.Month, PreferredDay);

            if (targetDate <= DueDate)
            {
                RealPaymentDate = targetDate;

            }
            if (targetDate > DueDate)
            {
                RealPaymentDate = ExpenseEntryDate;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            RealPaymentDate = ExpenseEntryDate;
            return;
        }

    }
}
