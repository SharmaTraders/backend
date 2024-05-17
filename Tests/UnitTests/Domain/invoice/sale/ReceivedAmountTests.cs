using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.sale;

public class ReceivedAmountTests
{
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetValidPositiveNumbers), MemberType = typeof(InvoiceFactory))]
    public void Sale_WithValidReceivedAmount_CanBeCreated(double validNumber)
    {
        // Arrange
        var saleEntity = new SaleEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            ReceivedAmount = validNumber,
            Sales = new List<SaleLineItem>(){ValidObjects.GetValidSaleLineItem()},
            TransportFee = 0,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        };
            
        // Act No exception is thrown
        Assert.Equal(validNumber, saleEntity.ReceivedAmount);
    }
        
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetInValidNumbers), MemberType = typeof(InvoiceFactory))]
    public void Sale_WithInValidReceivedAmount_CannotBeCreated(double invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>(() => new SaleEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            ReceivedAmount = invalidNumber,
            Sales = new List<SaleLineItem>(){ValidObjects.GetValidSaleLineItem()},
            TransportFee = 0,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        });
            
        // Assert
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("ReceivedAmount", StringComparison.OrdinalIgnoreCase));
    }
}