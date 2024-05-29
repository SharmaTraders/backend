using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Expense;

public class AmountTests
{
    [Theory]
    [MemberData(nameof(ExpenseFactory.GetValidAmounts), MemberType = typeof(ExpenseFactory))]
    public void Expense_WithValidAmount_CanBeCreated(double validAmount)
    {
        var expense = new ExpenseEntity
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Amount = validAmount,
            Category = new ExpenseCategoryEntity { Name = "Valid Category" }
        };
        Assert.Equal(validAmount, expense.Amount);
    }

    [Theory]
    [MemberData(nameof(ExpenseFactory.GetInvalidAmounts), MemberType = typeof(ExpenseFactory))]
    public void Expense_WithInvalidAmount_CannotBeCreated(double invalidAmount)
    {
        var exception = Assert.Throws<DomainValidationException>(() => new ExpenseEntity
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Amount = invalidAmount,
            Category = new ExpenseCategoryEntity { Name = "Valid Category" }
        });
        Assert.Equal("amount", exception.Type.ToLower());
    }
}