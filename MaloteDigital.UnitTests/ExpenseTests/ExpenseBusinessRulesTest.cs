using MaloteDigital.Domain.Entities;

namespace MaloteDigital.UnitTests.ExpenseTests
{
    public class ExpenseBusinessRulesTest
    {
        [Fact]
        public void Should_SuggestDueDate_When_PreferredPaymentDateIsZero()
        {
            var expense = new Expense
            {
                DueDate = new DateTime(2024, 7, 15),
                ExpenseEntryDate = new DateTime(2024, 7, 1)
            };

            expense.CalculatePreferredDate(0);


            Assert.Equal(expense.DueDate, expense.RealPaymentDate);

        }
        [Fact]
        public void Should_SuggestPreferredDate_When_PreferredDateIsBeforeDueDate()
        {
            var expense = new Expense
            {
                DueDate = new DateTime(2024, 7, 15),
                ExpenseEntryDate = new DateTime(2024, 7, 1)
            };

            expense.CalculatePreferredDate(4);

            Assert.Equal(new DateTime(2024, 7, 4), expense.RealPaymentDate);
        }

        [Fact]

        public void Should_SuggestEntryDate_When_PreferredDateIsAfterDueDate()
        {
            var expense = new Expense
            {
                DueDate = new DateTime(2024, 7, 15),
                ExpenseEntryDate = new DateTime(2024, 7, 1)
            };

            expense.CalculatePreferredDate(20);

            Assert.Equal(expense.ExpenseEntryDate, expense.RealPaymentDate);

        }
    }
}
