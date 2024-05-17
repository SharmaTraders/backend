using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.purchase;

public class RemarksTests
{
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetValidRemarks), MemberType = typeof(InvoiceFactory))]
    public void Purchase_WithValidRemarks_CanBeCreated(string validRemarks)
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
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = validRemarks
        };
        
        // Act No exception is thrown
        Assert.Equal(validRemarks, purchaseEntity.Remarks);
    }

    [Theory]
    [MemberData(nameof(InvoiceFactory.GetInvalidRemarks), MemberType = typeof(InvoiceFactory))]
    public void Purchase_WithInvalidRemarks_CanNotBeCreated(string inValidRemarks)
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
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = inValidRemarks
        });
        
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("Remarks", StringComparison.OrdinalIgnoreCase));
    }
    
}