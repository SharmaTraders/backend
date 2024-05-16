using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.income;

public class DateTests {
    [Theory]
    [MemberData(nameof(IncomeFactory.GetValidDates), MemberType = typeof(IncomeFactory))]
    public void Income_WithValidDate_CanBeCreated(DateOnly validDate) {
        // No exception is thrown
        IncomeEntity entity = new IncomeEntity() {
            Amount = 100,
            BillingParty = null!,
            Date = validDate,
            Remarks = ""
        };
        Assert.Equal(validDate, entity.Date);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInvalidDate), MemberType = typeof(ItemFactory))]
    public void Income_WithInValidDate_CannotBeCreated(DateOnly invalidDate) {
        var exception = Assert.Throws<DomainValidationException>(() => new IncomeEntity() {
            Amount = 100,
            BillingParty = null!,
            Date = invalidDate,
            Remarks = ""
        });
        Assert.Equal("date", exception.Type.ToLower());
    }

}