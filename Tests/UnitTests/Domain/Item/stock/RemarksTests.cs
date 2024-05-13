using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Item.stock;

public class RemarksTests {
    
    [Theory]
    [MemberData(nameof(ItemFactory.GetValidStockRemarks), MemberType = typeof(ItemFactory))]
    public void Stock_WithValidRemarks_CanBeCreated(string validRemarks) {
        // No exception is thrown
        Stock stock = new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = 10,
            ExpectedValuePerKilo = 10,
            EntryCategory = StockEntryCategory.Purchase,
            Remarks = validRemarks
        };
        Assert.Equal(validRemarks, stock.Remarks);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInvalidStockRemarks), MemberType = typeof(ItemFactory))]
    public void Stock_WithInValidRemarks_CannotBeCreated(string invalidRemarks) {
        // No exception is thrown
        var exception = Assert.Throws<DomainValidationException>( () => new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = 10,
            ExpectedValuePerKilo = 10,
            EntryCategory = StockEntryCategory.Purchase,
            Remarks = invalidRemarks
        });
        Assert.Equal("remarks", exception.Type.ToLower());
    }
}