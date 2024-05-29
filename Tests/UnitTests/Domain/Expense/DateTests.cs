using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Expense;

public class DateTests
{
    [Theory]
    [MemberData(nameof(ExpenseFactory.GetValidDates), MemberType = typeof(ExpenseFactory))]
    public void Expense_WithValidDate_CanBeCreated(DateOnly validDate)
    {
        var expense = new ExpenseEntity
        {
            Date = validDate,
            Amount = 100,
            Category = new ExpenseCategoryEntity { Name = "Valid Category" }
        };
        Assert.Equal(validDate, expense.Date);
    }

    [Theory]
    [MemberData(nameof(ExpenseFactory.GetInvalidDates), MemberType = typeof(ExpenseFactory))]
    public void Expense_WithInvalidDate_CannotBeCreated(DateOnly invalidDate)
    {
        var exception = Assert.Throws<DomainValidationException>(() => new ExpenseEntity
        {
            Date = invalidDate,
            Amount = 100,
            Category = new ExpenseCategoryEntity { Name = "Valid Category" }
        });
        Assert.Equal("date", exception.Type.ToLower());
    }
}