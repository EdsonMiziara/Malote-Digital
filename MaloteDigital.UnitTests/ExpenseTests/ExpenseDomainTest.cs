using MaloteDigital.Domain.Entities;
using MaloteDigital.Domain.Enums;
using MaloteDigital.Domain.Exceptions;
using Xunit;

namespace MaloteDigital.UnitTests.ExpenseTests;

public class ExpenseDomainTests
{
    [Fact]
    public void CalculatePreferredDate_ShouldSetDueDateAsPreferredDate_WhenCoringaDayIsZero()
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Amount = 150.00m,
            DueDate = new DateTime(2026, 06, 10), 
            Status = "Pendente"
        };
        int coringaDay = 0; 

        expense.CalculatePreferredDate(coringaDay);

        Assert.Equal(expense.DueDate, expense.RealPaymentDate);
    }

    [Fact]
    public void FinalizePayment_ShouldThrowBusinessException_WhenExpenseIsAlreadyPaid()
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Amount = 350.00m,
            DueDate = new DateTime(2026, 06, 10),
            Status = "Pago", 
            ActualPaymentDate = new DateTime(2026, 06, 05)
        };
        var newPaymentDate = new DateTime(2026, 06, 06); 

        var exception = Assert.Throws<BusinessException>(() => expense.FinalizePayment(newPaymentDate));

        Assert.Contains("já possui o status Pago", exception.Message);
    }

    [Fact]

    public void Expense_WhenCreated_ShouldHaveDefaultFixedTypeAndNullableObservation()
    {
        var expense = new Expense();

        Assert.Equal(ExpenseType.Fixed, expense.ExpenseType);
        Assert.Null(expense.Observation);
    }
}