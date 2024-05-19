using Domain.Entity;
using UnitTests.Factory;
using Xunit;

namespace UnitTests.Domain.invoice.sale.saleLine
{
    public class SalePriceTests
    {
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetValidPositiveNumbers), MemberType = typeof(InvoiceFactory))]
        public void SaleLineItem_WithValidPrice_CanBeCreated(double validNumber)
        {
            // Arrange
            var saleLineItem = new SaleLineItem
            {
                Id = Guid.NewGuid(),
                ItemEntity = ValidObjects.GetValidItem(),
                Quantity = 10,
                Price = validNumber,
                Report = 5.77
            };
            
            // Act No exception is thrown
            Assert.Equal(validNumber, saleLineItem.Price);
        }
        
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetInValidNumbersInclZero), MemberType = typeof(InvoiceFactory))]
        public void SaleLineItem_WithInValidPrice_CannotBeCreated(double invalidNumber)
        {
            // Arrange
            var exception = Assert.Throws<DomainValidationException>(() => new SaleLineItem
            {
                Id = Guid.NewGuid(),
                ItemEntity = ValidObjects.GetValidItem(),
                Quantity = 10,
                Price = invalidNumber,
                Report = 5.77
            });
            
            // Assert
            Assert.NotEmpty(exception.Message);
            Assert.True(exception.Type.Equals("Price", StringComparison.OrdinalIgnoreCase));
        }
    }
}