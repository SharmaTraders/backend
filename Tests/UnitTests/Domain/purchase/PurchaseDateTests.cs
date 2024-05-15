using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.purchase;

public class PurchaseDateTests
{
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetValidPurchaseDates), MemberType = typeof(PurchaseFactory))]
    public void Purchase_WithValidDate_CanBeCreated(DateOnly validDate)
    {
        // Arrange
        var purchaseEntity = new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = validDate,
            PaidAmount = 0,
            Purchases = new List<PurchaseLineItem>(){ValidObjects.GetValidPurchaseLineItem()},
            TransportFee = 0,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        };
        
        // Act No exception is thrown
        Assert.Equal(validDate, purchaseEntity.Date);
    }
    
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetInValidPurchaseDates), MemberType = typeof(PurchaseFactory))]
    public void Purchase_WithInValidDate_CannotBeCreated(DateOnly invalidDate)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>( ()  => new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = invalidDate,
            PaidAmount = 0,
            Purchases = new List<PurchaseLineItem>(){ValidObjects.GetValidPurchaseLineItem()},
            TransportFee = 0,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        });
        
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("date", StringComparison.OrdinalIgnoreCase));
    }
    
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetInvalidDateTypes), MemberType = typeof(PurchaseFactory))]
    public void Purchase_WithInvalidDateType_ThrowsException(object invalidDate)
    {
        // Arrange
        var billingParty = ValidObjects.GetValidBillingParty(); 

        // Act & Assert
        var exception = Assert.Throws<InvalidCastException>(() =>
        {
            var purchaseEntity = new PurchaseEntity
            {
                Id = Guid.NewGuid(),
                BillingParty = billingParty,
                // This cast should throw an exception
                Date = (DateOnly)Convert.ChangeType(invalidDate, typeof(DateOnly)),
                PaidAmount = 0,
                Purchases = new List<PurchaseLineItem> { ValidObjects.GetValidPurchaseLineItem() },
                TransportFee = 0,
                VatAmount = 0,
                InvoiceNumber = 0,
                Remarks = "Test Remarks"
            };
        });

        Assert.NotEmpty(exception.Message);
    }
}