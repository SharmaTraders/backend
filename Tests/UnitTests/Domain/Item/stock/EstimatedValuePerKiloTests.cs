using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Item.stock;

public class EstimatedValuePerKiloTests {
    [Theory]
    [MemberData(nameof(ItemFactory.GetValidValuePerKilo), MemberType = typeof(ItemFactory))]
    public void Stock_WithValidValuePerKilo_CanBeCreated(double validValuePerKilo) {
        // No exception is thrown
       Stock stock = new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = 10,
            ExpectedValuePerKilo = validValuePerKilo,
            EntryCategory = StockEntryCategory.Purchase,
        };
        Assert.Equal(validValuePerKilo, stock.ExpectedValuePerKilo);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInValidValuePerKilo), MemberType = typeof(ItemFactory))]
    public void Stock_WithInValidValuePerKilo_CannotBeCreated(double invalidValuePerKilo) {
        // No exception is thrown
        var exception = Assert.Throws<DomainValidationException>( () => new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = 10,
            ExpectedValuePerKilo = invalidValuePerKilo,
            EntryCategory = StockEntryCategory.Purchase,
        });
        Assert.Equal("expectedvalueperkilo", exception.Type.ToLower());
    }
}