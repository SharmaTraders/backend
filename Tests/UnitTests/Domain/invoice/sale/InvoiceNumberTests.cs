using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.sale;

public class InvoiceNumberTests
{
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetValidInvoiceNumbers), MemberType = typeof(InvoiceFactory))]
    public void Sale_WithValidInvoiceNumber_CanBeCreated(int validNumber)
    {
        // Arrange
        var saleEntity = new SaleEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            ReceivedAmount = 0,
            Sales = new List<SaleLineItem>(){ValidObjects.GetValidSaleLineItem()},
            TransportFee = 0,
            VatAmount = 0,
            InvoiceNumber = validNumber,
            Remarks = "Test Remarks"
        };
            
        // Act No exception is thrown
        Assert.Equal(validNumber, saleEntity.InvoiceNumber);
    }
        
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetInValidInvoiceNumbers), MemberType = typeof(InvoiceFactory))]
    public void Sale_WithInValidInvoiceNumber_CannotBeCreated(int invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>(() => new SaleEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            ReceivedAmount = 0,
            Sales = new List<SaleLineItem>(){ValidObjects.GetValidSaleLineItem()},
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