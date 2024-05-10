using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Item;

public class ValuePerKiloTests {
    [Theory]
    [MemberData(nameof(ItemFactory.GetValidValuePerKilo), MemberType = typeof(ItemFactory))]

    public void Item_WithValidOpeningStock_CanBeCreated(double valuePerKilo) {
        // No exception is thrown
        ItemEntity itemEntity = new ItemEntity {
            Name = "Test Name",
            CurrentStockAmount = 50,
            CurrentEstimatedStockValuePerKilo = valuePerKilo,
        };
        Assert.Equal(valuePerKilo, itemEntity.CurrentEstimatedStockValuePerKilo);
    }


    [Theory]
    [MemberData(nameof(ItemFactory.GetInValidValuePerKilo), MemberType = typeof(ItemFactory))]
    public void Item_WithInValidOpeningStock_CannotBeCreated(double invalidValuePerKilo) {
        var exception = Assert.Throws<DomainValidationException>(() => new ItemEntity {
            Name = "Test Name",
            CurrentStockAmount = 50,
            CurrentEstimatedStockValuePerKilo = invalidValuePerKilo,
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("valueperkilo", StringComparison.OrdinalIgnoreCase));
    }
}