using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.sale
{
    public class SaleBillingItemTests
    {
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetValidSaleLineItems), MemberType = typeof(InvoiceFactory))]
        public void Sale_WithValidSaleLineItem_CanBeCreated(SaleLineItem validSaleLineItem)
        {
            // Arrange
            var saleEntity = new SaleEntity
            {
                Id = Guid.NewGuid(),
                BillingParty = ValidObjects.GetValidBillingParty(),
                Date = DateOnly.FromDateTime(DateTime.Now),
                ReceivedAmount = 0,
                Sales = new List<SaleLineItem>() { validSaleLineItem },
                TransportFee = 0,
                VatAmount = 0,
                InvoiceNumber = 0,
                Remarks = "Test Remarks"
            };

            // Act No exception is thrown
            Assert.Contains(validSaleLineItem, saleEntity.Sales);
        }
    }
}