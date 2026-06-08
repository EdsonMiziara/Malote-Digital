using MaloteDigital.Domain.Enums;
using MaloteDigital.Domain.Exceptions;

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
    public DateTime? IssueDate { get; set; }
    public DateTime? ActualPaymentDate { get; set; }
    public string FileHash { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendente";
    public string DetailedDescription { get; set; } = string.Empty;
    public string? Observation { get; set; }
    public ExpenseType ExpenseType { get; set; } = ExpenseType.Fixed;
    public string? PdfUrl { get; set;}


    /// <summary>
    /// Efetua a baixa definitiva da despesa com base no extrato bancário.
    /// </summary>
    /// <exception cref="BusinessException">Lançada caso a despesa já esteja paga.</exception>
    public void FinalizePayment(DateTime paymentDate)
    {
        if (Status == "Pago")
        {
            throw new BusinessException($"A despesa com o ID {Id} já possui o status Pago. Operação de conciliação abortada.");
        }

        Status = "Pago";
        ActualPaymentDate = paymentDate;
    }
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
