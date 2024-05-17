using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.purchase;

public class InvoiceNumberTests
{
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetValidInvoiceNumbers), MemberType = typeof(InvoiceFactory))]
    public void Purchase_WithValidInvoiceNumber_CanBeCreated(int validNumber)
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
            InvoiceNumber = validNumber,
            Remarks = "Test Remarks"
        };
        
        // Act No exception is thrown
        Assert.Equal(validNumber, purchaseEntity.InvoiceNumber);
    }
    
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetInValidInvoiceNumbers), MemberType = typeof(InvoiceFactory))]
    public void Purchase_WithInValidInvoiceNumber_CannotBeCreated(int invalidNumber)
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
            InvoiceNumber = invalidNumber,
            Remarks = "Test Remarks"
        });
        
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("InvoiceNumber", StringComparison.OrdinalIgnoreCase));
    }
}