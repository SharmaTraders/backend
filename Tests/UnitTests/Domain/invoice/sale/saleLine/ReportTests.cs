using Domain.Entity;
using UnitTests.Factory;
using Xunit;

namespace UnitTests.Domain.invoice.sale.saleLine
{
    public class SaleReportTests
    {
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetValidNumberInclZero), MemberType = typeof(InvoiceFactory))]
        public void SaleLineItem_WithValidReport_CanBeCreated(double validNumber)
        {
            // Arrange
            var saleLineItem = new SaleLineItem
            {
                Id = Guid.NewGuid(),
                ItemEntity = ValidObjects.GetValidItem(),
                Quantity = 10,
                Price = 20.5,
                Report = validNumber
            };
            
            // Act No exception is thrown
            Assert.Equal(validNumber, saleLineItem.Report);
        }
        
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetInValidNumbers), MemberType = typeof(InvoiceFactory))]
        public void SaleLineItem_WithInValidReport_CannotBeCreated(double invalidNumber)
        {
            // Arrange
            var exception = Assert.Throws<DomainValidationException>(() => new SaleLineItem
            {
                Id = Guid.NewGuid(),
                ItemEntity = ValidObjects.GetValidItem(),
                Quantity = 10,
                Price = 20.5,
                Report = invalidNumber
            });
            
            // Assert
            Assert.NotEmpty(exception.Message);
            Assert.True(exception.Type.Equals("Report", StringComparison.OrdinalIgnoreCase));
        }
    }
}