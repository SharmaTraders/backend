using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Item;

public class OpeningStockTests {
    [Theory]
    [MemberData(nameof(ItemFactory.GetValidOpeningStocks), MemberType = typeof(ItemFactory))]
    public void Item_WithValidOpeningStock_CanBeCreated(double validOpeningStock) {
        // No exception is thrown
        ItemEntity itemEntity = new ItemEntity {
            Name = "Test Name",
            CurrentStockAmount = validOpeningStock,
            CurrentEstimatedStockValuePerKilo = 50,
        };
        Assert.Equal(validOpeningStock, itemEntity.CurrentStockAmount);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInValidOpeningStocks), MemberType = typeof(ItemFactory))]
    public void Item_WithInValidOpeningStock_CannotBeCreated(double invalidOpeningStock) {
        var exception = Assert.Throws<DomainValidationException>(() => new ItemEntity {
            Name = "Test Name",
            CurrentStockAmount = invalidOpeningStock,
            CurrentEstimatedStockValuePerKilo = 50,
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("stockamount", StringComparison.OrdinalIgnoreCase));
    }
}