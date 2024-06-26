﻿using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.invoice.purchase.purchaseLine;

public class PriceTests
{
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetValidPositiveNumbers), MemberType = typeof(InvoiceFactory))]
    public void PurchaseLineItem_WithValidPrice_CanBeCreated(double validNumber)
    {
        // Arrange
        var purchaseLineItem = new PurchaseLineItem
        {
            Id = Guid.NewGuid(),
            ItemEntity = ValidObjects.GetValidItem(),
            Quantity = 10,
            Price = validNumber,
            Report = 5.77
        };
        
        // Act No exception is thrown
        Assert.Equal(validNumber, purchaseLineItem.Price);
    }
    
    [Theory]
    [MemberData(nameof(InvoiceFactory.GetInValidNumbersInclZero), MemberType = typeof(InvoiceFactory))]
    public void PurchaseLineItem_WithInValidPrice_CannotBeCreated(double invalidNumber)
    {
        // Arrange
        var exception = Assert.Throws<DomainValidationException>( ()  => new PurchaseLineItem
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