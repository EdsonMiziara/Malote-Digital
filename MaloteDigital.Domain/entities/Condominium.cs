using System.Runtime.InteropServices;

namespace MaloteDigital.Domain.Entities;

public class Condominium
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int PreferredPaymentDate { get; set; }
    public ICollection<Expense> Expenses {  get; set; } 
}
