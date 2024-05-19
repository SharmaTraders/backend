using Domain.Entity;
using UnitTests.Factory;
using Xunit;

namespace UnitTests.Domain.invoice.sale.saleLine
{
    public class SaleQuantityTests
    {
        [Theory]
        [MemberData(nameof(InvoiceFactory.GetValidPositiveNumbers), MemberType = typeof(InvoiceFactory))]
        public void SaleLineItem_WithValidQuantity_CanBeCreated(double validNumber)
        {
            // Arrange
            var saleLineItem = new SaleLineItem
            {
                Id = Guid.NewGuid(),
                ItemEntity = ValidObjects.GetValidItem(),
                Quantity = validNumber,
                Price = 20.5,
                Report = 5.77
            };
            
            // Act No exception is thrown
            Assert.Equal(validNumber, saleLineItem.Quantity);
        }

        [Theory]
        [MemberData(nameof(InvoiceFactory.GetInValidNumbersInclZero), MemberType = typeof(InvoiceFactory))]
        public void SaleLineItem_WithInValidQuantity_CannotBeCreated(double invalidNumber)
        {
            // Arrange
            var exception = Assert.Throws<DomainValidationException>(() => new SaleLineItem
            {
                Id = Guid.NewGuid(),
                ItemEntity = ValidObjects.GetValidItem(),
                Quantity = invalidNumber,
                Price = 20.5,
                Report = 5.77
            });
            
            // Assert
            Assert.NotEmpty(exception.Message);
            Assert.True(exception.Type.Equals("Quantity", StringComparison.OrdinalIgnoreCase));
        }
    }
}