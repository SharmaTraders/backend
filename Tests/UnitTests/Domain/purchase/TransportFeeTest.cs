using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.purchase;

public class TransportFeeTest
{
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetValidNumberInclZero), MemberType = typeof(PurchaseFactory))]
    public void Purchase_WithValidTransportFee_CanBeCreated(double validNumber)
    {
        // Arrange
        var purchaseEntity = new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            PaidAmount = 0,
            Purchases = new List<PurchaseLineItem>(){ValidObjects.GetValidPurchaseLineItem()},
            TransportFee = validNumber,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        };
        
        // Act No exception is thrown
        Assert.Equal(validNumber, purchaseEntity.TransportFee);
    }
    
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetInValidNumbers), MemberType = typeof(PurchaseFactory))]
    public void Purchase_WithInValidTransportFee_CannotBeCreated(double invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>( ()  => new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            PaidAmount = 0,
            Purchases = new List<PurchaseLineItem>(){ValidObjects.GetValidPurchaseLineItem()},
            TransportFee = invalidNumber,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        });
        
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("TransportFee", StringComparison.OrdinalIgnoreCase));
    }
}