using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Expense
{
    public class ExpenseCategoryEntityTests
    {
        [Theory]
        [MemberData(nameof(ExpenseCategoryFactory.GetValidExpenseCategoryNames),
            MemberType = typeof(ExpenseCategoryFactory))]
        public void ExpenseCategory_WithValidName_CanBeCreated(string validName)
        {
            var expenseCategory = new ExpenseCategoryEntity
            {
                Name = validName
            };
            Assert.Equal(validName.Trim(), expenseCategory.Name);
        }

        [Theory]
        [MemberData(nameof(ExpenseCategoryFactory.GetInvalidExpenseCategoryNames),
            MemberType = typeof(ExpenseCategoryFactory))]
        public void ExpenseCategory_WithInvalidName_CannotBeCreated(string invalidName)
        {
            var exception = Assert.Throws<DomainValidationException>(() => new ExpenseCategoryEntity
            {
                Name = invalidName
            });
            Assert.NotEmpty(exception.Message);
            Assert.True(exception.Type.Equals("Name", StringComparison.OrdinalIgnoreCase));
        }
    }
}