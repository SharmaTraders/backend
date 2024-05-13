using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Item;

public class NameTests {
    
    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]
    public void Item_WithValidItemName_CanBeCreated(string validName) {
        // No exception is thrown
        ItemEntity itemEntity = new ItemEntity {
            Name = validName,
            CurrentStockAmount = 5,
            CurrentEstimatedStockValuePerKilo = 50
        };
        Assert.Equal(validName, itemEntity.Name);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInValidItemNames), MemberType = typeof(ItemFactory))]
    public void Item_WithInValidItemName_CannotBeCreated(string invalidName) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new ItemEntity {
            Name = invalidName,
            CurrentStockAmount = 5,
            CurrentEstimatedStockValuePerKilo = 50
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("itemname", StringComparison.OrdinalIgnoreCase));
    }
}