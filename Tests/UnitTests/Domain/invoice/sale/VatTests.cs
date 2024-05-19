using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.sale;

public class VatTests
{
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetValidNumberInclZero), MemberType = typeof(InvoiceFactory))]
    public void Sale_WithValidVatAmount_CanBeCreated(double validNumber)
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
            VatAmount = validNumber,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        };
            
        // Act No exception is thrown
        Assert.Equal(validNumber, saleEntity.VatAmount);
    }
        
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetInValidNumbers), MemberType = typeof(InvoiceFactory))]
    public void Sale_WithInValidVatAmount_CannotBeCreated(double invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>( ()  => new SaleEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            ReceivedAmount = 0,
            Sales = new List<SaleLineItem>(){ValidObjects.GetValidSaleLineItem()},
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