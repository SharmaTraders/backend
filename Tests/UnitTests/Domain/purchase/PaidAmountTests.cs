using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.purchase;

public class PaidAmountTests
{
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetValidPositiveNumbers), MemberType = typeof(PurchaseFactory))]
    public void Purchase_WithValidPaidAmount_CanBeCreated(double validNumber)
    {
        // Arrange
        var purchaseEntity = new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            PaidAmount = validNumber,
            Purchases = new List<PurchaseLineItem>(){ValidObjects.GetValidPurchaseLineItem()},
            TransportFee = 0,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        };
        
        // Act No exception is thrown
        Assert.Equal(validNumber, purchaseEntity.PaidAmount);
    }
    
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetInValidNumbers), MemberType = typeof(PurchaseFactory))]
    public void Purchase_WithInValidPaidAmount_CannotBeCreated(double invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>( ()  => new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            PaidAmount = invalidNumber,
            Purchases = new List<PurchaseLineItem>(){ValidObjects.GetValidPurchaseLineItem()},
            TransportFee = 0,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        });
        
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("PaidAmount", StringComparison.OrdinalIgnoreCase));
    }
}