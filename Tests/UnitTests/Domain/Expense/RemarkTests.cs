using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Expense;

public class RemarkTests
{
    [Theory]
    [MemberData(nameof(ExpenseFactory.GetValidRemarks), MemberType = typeof(ExpenseFactory))]
    public void Expense_WithValidRemarks_CanBeCreated(string validRemarks)
    {
        var expense = new ExpenseEntity
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Amount = 100,
            Remarks = validRemarks,
            Category = new ExpenseCategoryEntity { Name = "Valid Category" }
        };
        Assert.Equal(validRemarks?.Trim(), expense.Remarks);
    }

    [Theory]
    [MemberData(nameof(ExpenseFactory.GetInvalidRemarks), MemberType = typeof(ExpenseFactory))]
    public void Expense_WithInvalidRemarks_CannotBeCreated(string invalidRemarks)
    {
        var exception = Assert.Throws<DomainValidationException>(() => new ExpenseEntity
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Amount = 100,
            Remarks = invalidRemarks,
            Category = new ExpenseCategoryEntity { Name = "Valid Category" }
        });
        Assert.Equal("remarks", exception.Type.ToLower());
    }
}