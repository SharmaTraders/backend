using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Item.stock;

public class WeightTests {
    [Theory]
    [MemberData(nameof(ItemFactory.GetValidStockWeight), MemberType = typeof(ItemFactory))]
    public void Stock_WithValidValuePerKilo_CanBeCreated(double validStockWeight) {
        // No exception is thrown
        Stock stock = new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = validStockWeight,
            ExpectedValuePerKilo = 10,
            EntryCategory = StockEntryCategory.Purchase,
        };
        Assert.Equal(validStockWeight, stock.Weight);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInvalidStockWeight), MemberType = typeof(ItemFactory))]
    public void Stock_WithInValidValuePerKilo_CannotBeCreated(double invalidStockWeight) {
        // No exception is thrown
        var exception = Assert.Throws<DomainValidationException>( () => new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = invalidStockWeight,
            ExpectedValuePerKilo = 10,
            EntryCategory = StockEntryCategory.Purchase,
        });
        Assert.Equal("weight", exception.Type.ToLower());
    }
}