using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.sale
{
    public class SaleDateTests
    {
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetValidInvoiceDates), MemberType = typeof(InvoiceFactory))]
        public void Sale_WithValidDate_CanBeCreated(DateOnly validDate)
        {
            // Arrange
            var saleEntity = new SaleEntity
            {
                Id = Guid.NewGuid(),
                BillingParty = ValidObjects.GetValidBillingParty(),
                Date = validDate,
                ReceivedAmount = 0,
                Sales = new List<SaleLineItem>(){ValidObjects.GetValidSaleLineItem()},
                TransportFee = 0,
                VatAmount = 0,
                InvoiceNumber = 0,
                Remarks = "Test Remarks"
            };
            
            // Act No exception is thrown
            Assert.Equal(validDate, saleEntity.Date);
        }
        
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetInValidInvoiceDates), MemberType = typeof(InvoiceFactory))]
        public void Sale_WithInValidDate_CannotBeCreated(DateOnly invalidDate)
        {
            // Arrange
            var exception = Assert.Throws<DomainValidationException>(() => new SaleEntity
            {
                Id = Guid.NewGuid(),
                BillingParty = ValidObjects.GetValidBillingParty(),
                Date = invalidDate,
                ReceivedAmount = 0,
                Sales = new List<SaleLineItem>(){ValidObjects.GetValidSaleLineItem()},
                TransportFee = 0,
                VatAmount = 0,
                InvoiceNumber = 0,
                Remarks = "Test Remarks"
            });
            
            // Assert
            Assert.NotEmpty(exception.Message);
            Assert.True(exception.Type.Equals("Date", StringComparison.OrdinalIgnoreCase));
        }
        
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetInvalidDateTypes), MemberType = typeof(InvoiceFactory))]
        public void Sale_WithInvalidDateType_ThrowsException(object invalidDate)
        {
            // Arrange
            var billingParty = ValidObjects.GetValidBillingParty(); 

            // Act & Assert
            var exception = Assert.Throws<InvalidCastException>(() =>
            {
                var saleEntity = new SaleEntity
                {
                    Id = Guid.NewGuid(),
                    BillingParty = billingParty,
                    // This cast should throw an exception
                    Date = (DateOnly)Convert.ChangeType(invalidDate, typeof(DateOnly)),
                    ReceivedAmount = 0,
                    Sales = new List<SaleLineItem> { ValidObjects.GetValidSaleLineItem() },
                    TransportFee = 0,
                    VatAmount = 0,
                    InvoiceNumber = 0,
                    Remarks = "Test Remarks"
                };
            });

            Assert.NotEmpty(exception.Message);
        }
    }
}
