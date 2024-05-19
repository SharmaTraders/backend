using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.purchase;

public class VatTests
{
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetValidNumberInclZero), MemberType = typeof(InvoiceFactory))]
    public void Purchase_WithValidVatAmount_CanBeCreated(double validNumber)
    {
        // Arrange
        var purchaseEntity = new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            PaidAmount = 0,
            Purchases = new List<PurchaseLineItem>(){ValidObjects.GetValidPurchaseLineItem()},
            TransportFee = 0,
            VatAmount = validNumber,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        };
        
        // Act No exception is thrown
        Assert.Equal(validNumber, purchaseEntity.VatAmount);
    }
    
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetInValidNumbers), MemberType = typeof(InvoiceFactory))]
    public void Purchase_WithInValidVatAmount_CannotBeCreated(double invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>( ()  => new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            PaidAmount = 0,
            Purchases = new List<PurchaseLineItem>(){ValidObjects.GetValidPurchaseLineItem()},
            TransportFee = 0,
            VatAmount = invalidNumber,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        });
        
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("VatAmount", StringComparison.OrdinalIgnoreCase));
    }
}