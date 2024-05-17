using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.purchase.purchaseLine;

public class ReportTests
{
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetValidNumberInclZero), MemberType = typeof(InvoiceFactory))]
    public void PurchaseLineItem_WithValidReport_CanBeCreated(double validNumber)
    {
        // Arrange
        var purchaseLineItem = new PurchaseLineItem
        {
            Id = Guid.NewGuid(),
            ItemEntity = ValidObjects.GetValidItem(),
            Quantity = 10,
            Price = 20.5,
            Report = validNumber
        };
        
        // Act No exception is thrown
        Assert.Equal(validNumber, purchaseLineItem.Report);
    }
    
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetInValidNumbers), MemberType = typeof(InvoiceFactory))]
    public void PurchaseLineItem_WithInValidReport_CannotBeCreated(double invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>( ()  => new PurchaseLineItem
        {
            Id = Guid.NewGuid(),
            ItemEntity = ValidObjects.GetValidItem(),
            Quantity = 10,
            Price = 20.5,
            Report = invalidNumber
        });
        
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("Report", StringComparison.OrdinalIgnoreCase));
    }
}