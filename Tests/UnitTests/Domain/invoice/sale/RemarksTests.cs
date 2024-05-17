using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.sale
{
    public class RemarksTests
    {
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetValidRemarks), MemberType = typeof(InvoiceFactory))]
        public void Sale_WithValidRemarks_CanBeCreated(string validRemarks)
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
                InvoiceNumber = 0,
                Remarks = validRemarks
            };
            
            // Act No exception is thrown
            Assert.Equal(validRemarks, saleEntity.Remarks);
        }

        [Theory]
        [MemberData(nameof(InvoiceFactory.GetInvalidRemarks), MemberType = typeof(InvoiceFactory))]
        public void Sale_WithInvalidRemarks_CannotBeCreated(string invalidRemarks)
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
                InvoiceNumber = 0,
                Remarks = invalidRemarks
            });
            
            // Assert
            Assert.NotEmpty(exception.Message);
            Assert.True(exception.Type.Equals("Remarks", StringComparison.OrdinalIgnoreCase));
        }
    }
}