using Domain.Entity;

namespace UnitTests.Factory;

internal static class PurchaseFactory
{
    public static IEnumerable<object[]> GetValidPurchaseDates()
    {
        return new List<object[]>
        {
            new object[] { new DateOnly(2023, 1, 1) },
            new object[] { new DateOnly(2024, 4, 10) },
            new object[] { DateOnly.FromDateTime(DateTime.Now) }
        };
    }
    
    public static IEnumerable<object[]> GetInValidPurchaseDates()
    {
        return new List<object[]>
        {
            new object[] { DateOnly.MaxValue },
            new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(1)) },
        };
    }
    
    public static IEnumerable<object[]> GetInvalidDateTypes()
    {
        return new List<object[]>
        {
            new object[] { "asdsadasda" },
            new object[] { 12345 },
            new object[] { new object() }
        };
    }
    
    public static IEnumerable<object[]> GetValidPurchaseLineItems()
    {
        return new List<object[]>
        {
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = GetValidItem(), Quantity = 10, Price = 20.5, Report = 5.77 } },
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = GetValidItem(), Quantity = 3333, Price = 20.99, Report = null } },
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = GetValidItem(), Quantity = 1, Price = 0.5, Report = 0 } }
        };
    }
    
    public static IEnumerable<object[]> GetInvalidPurchaseLineItems()
    {
        return new List<object[]>
        {
            new object[] { new PurchaseLineItem() }, // Empty PurchaseLineItem
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = GetValidItem(), Quantity = -1, Price = 20, Report = 5 } }, // Negative Quantity
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = GetValidItem(), Quantity = 10, Price = -1, Report = 5 } }, // Negative Price
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = GetValidItem(), Quantity = 10, Price = -20, Report = -1 } }, // Negative Report
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = GetValidItem(), Quantity = 0, Price = 20, Report = 1 } }, // Zero Quantity
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = GetValidItem(), Quantity = 5, Price = 0, Report = 1 } } // Zero price
            
        };
    }
    
    private static ItemEntity GetValidItem()
    {
        return new ItemEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Item",
            CurrentStockAmount = 100,
            CurrentEstimatedStockValuePerKilo = 50
        };
    }
    
    // For vat, transport, report , paidAmount ,invoiceNumber can be 0 or null 
    
    public static IEnumerable<object[]> GetValidNumberInclZero()
    {
        return new List<object[]>
        {
            new object[] { 0.00 },
            new object[] { 1.00 },
            new object[] { 100.55 },
            new object[] { 50.25 },
            new object[] { (double?)null }
        };
    }
    
    // For quantity, price
    public static IEnumerable<object[]> GetValidPositiveNumber()
    {
        return new List<object[]>
        {
            new object[] { 1 },
            new object[] { 100 },
            new object[] { 50.25 }
        };
    }    
    
    public static IEnumerable<object[]> GetInValidNumber()
    {
        return new List<object[]>
        {
            new object[] { -1 },
            new object[] { -100 },
            new object[] { -10.54 }
        };
    }
}