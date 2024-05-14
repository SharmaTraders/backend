using Domain.Entity;

namespace UnitTests;

public class ValidObjects
{
    public static PurchaseLineItem GetValidPurchaseLineItem()
    {
        return new PurchaseLineItem()
        {
            Id = Guid.NewGuid(),
            ItemEntity = GetValidItem(),
            Price = 10,
            Quantity = 1,
            Report = 0.00
        };
    }

    public static ItemEntity GetValidItem()
    {
        return new ItemEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            CurrentEstimatedStockValuePerKilo = 50,
            CurrentStockAmount = 0
        };
    }

    public static BillingPartyEntity GetValidBillingParty()
    {
        return new BillingPartyEntity()
        {
            Address = "Test Address",
            Balance = 12.12,
            Email = "tesest@test.mail",
            Id = Guid.NewGuid(),
            Name = "Test Name",
            PhoneNumber = "1234567890",
            VatNumber = "1234567890"
        };
    }
}