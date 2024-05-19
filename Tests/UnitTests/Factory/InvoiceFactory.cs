using Domain.Entity;

namespace UnitTests.Factory;

internal static class InvoiceFactory
{
    public static IEnumerable<object[]> GetValidInvoiceDates()
    {
        return new List<object[]>
        {
            new object[] { new DateOnly(2023, 1, 1) },
            new object[] { new DateOnly(2024, 4, 10) },
            new object[] { DateOnly.FromDateTime(DateTime.Now) }
        };
    }
    
    public static IEnumerable<object[]> GetInValidInvoiceDates()
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
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = ValidObjects.GetValidItem(), Quantity = 10, Price = 20.5, Report = 5.77 } },
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = ValidObjects.GetValidItem(), Quantity = 3333, Price = 20.99, Report = null } },
            new object[] { new PurchaseLineItem { Id = Guid.NewGuid(), ItemEntity = ValidObjects.GetValidItem(), Quantity = 1, Price = 0.5, Report = 0 } }
        };
    }
    
    public static IEnumerable<object[]> GetValidSaleLineItems()
    {
        return new List<object[]>
        {
            new object[] { new SaleLineItem { Id = Guid.NewGuid(), ItemEntity = ValidObjects.GetValidItem(), Quantity = 10, Price = 20.5, Report = 5.77 } },
            new object[] { new SaleLineItem { Id = Guid.NewGuid(), ItemEntity = ValidObjects.GetValidItem(), Quantity = 3333, Price = 20.99, Report = null } },
            new object[] { new SaleLineItem { Id = Guid.NewGuid(), ItemEntity = ValidObjects.GetValidItem(), Quantity = 1, Price = 0.5, Report = 0 } }
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
    public static IEnumerable<object[]> GetValidPositiveNumbers()
    {
        return new List<object[]>
        {
            new object[] { 1 },
            new object[] { 100 },
            new object[] { 50.25 }
        };
    }    
    
    public static IEnumerable<object[]> GetInValidNumbers()
    {
        return new List<object[]>
        {
            new object[] { -1 },
            new object[] { -100 },
            new object[] { -10.54 },
            new object[] { 0.0005 }
        };
    }
    
    public static IEnumerable<object[]> GetInValidNumbersInclZero()
    {
        return new List<object[]>
        {
            new object[] { -1 },
            new object[] { -100 },
            new object[] { -10.54 },
            new object[] { 0.0005 },
            new object[] { 0 }
        };
    }
    
    public static IEnumerable<object[]> GetValidInvoiceNumbers()
    {
        return new List<object[]>
        {
            new object[] { 0 },
            new object[] { 1 },
            new object[] { 100 },
            new object[] { 50 }
        };
    }
    
    public static IEnumerable<object[]> GetInValidInvoiceNumbers()
    {
        return new List<object[]>
        {
            new object[] { -1 },
            new object[] { -100 },
            new object[] { -10 },
        };
    }
    
    public static IEnumerable<object[]> GetValidRemarks()
    {
        return new List<object[]>
        {
            new object[] { null },
            new object[] { "" },
            new object[] { "a".PadRight(20, 'a') },
            new object[] { "a".PadRight(500, 'a') }
        };
    }
    
    public static IEnumerable<object[]> GetInvalidRemarks()
    {
        return new List<object[]>
        {
            new object[] { "a".PadRight(501, 'a') },
            new object[] { "a".PadRight(1000, 'a') }
        };
    }
}