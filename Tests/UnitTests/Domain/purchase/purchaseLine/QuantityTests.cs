using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.purchase;

public class QuantityTests
{
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetValidPositiveNumbers), MemberType = typeof(PurchaseFactory))]
public void PurchaseLineItem_WithValidQuantity_CanBeCreated(double validNumber)
    {
        // Arrange
        var purchaseLineItem = new PurchaseLineItem
        {
            Id = Guid.NewGuid(),
            ItemEntity = ValidObjects.GetValidItem(),
            Quantity = validNumber,
            Price = 20.5,
            Report = 5.77
        };
        
        // Act No exception is thrown
        Assert.Equal(validNumber, purchaseLineItem.Quantity);
    }

    [Theory]
    [MemberData(nameof(PurchaseFactory.GetInValidNumbersInclZero), MemberType = typeof(PurchaseFactory))]
    public void PurchaseLineItem_WithInValidQuantity_CannotBeCreated(double invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>( ()  => new PurchaseLineItem
        {
            Id = Guid.NewGuid(),
            ItemEntity = ValidObjects.GetValidItem(),
            Quantity = invalidNumber,
            Price = 20.5,
            Report = 5.77
        });
        
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("Quantity", StringComparison.OrdinalIgnoreCase));
    }
}