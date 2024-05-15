using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Item.stock;

public class DateTests {
    [Theory]
    [MemberData(nameof(ItemFactory.GetValidDate), MemberType = typeof(ItemFactory))]
    public void Stock_WithValidDate_CanBeCreated(DateOnly validDate) {
        // No exception is thrown
        Stock stock = new Stock {
            Date = validDate,
            Weight = 10,
            ExpectedValuePerKilo = 10,
            EntryCategory = StockEntryCategory.Purchase,
            Remarks = "Test Remarks",
        };
        Assert.Equal(validDate, stock.Date);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInvalidDate), MemberType = typeof(ItemFactory))]
    public void Stock_WithInValidDate_CannotBeCreated(DateOnly invalidDate) {
        // No exception is thrown
        var exception = Assert.Throws<DomainValidationException>(() => new Stock {
            Date = invalidDate,
            Weight = 10,
            ExpectedValuePerKilo = 10,
            EntryCategory = StockEntryCategory.Purchase,
            Remarks = "Test Remarks",
        });
        Assert.Equal("date", exception.Type.ToLower());
    }


}