using Domain.DomainServices;
using Domain.Entity;

namespace UnitTests.Domain.DomainService;

public class AddPurchaseServiceTests {
    [Fact]
    public void AddPurchaseService_IncreasesStock_AndDecreasesBillingPartyBalance() {
        // Arrange
        ItemEntity itemEntity = new ItemEntity() {
            CurrentStockAmount = 10,
            CurrentEstimatedStockValuePerKilo = 10,
            Name = "test",
        };

        BillingPartyEntity billingPartyEntity = new BillingPartyEntity() {
            Address = "Test" ,
            Balance = 10_000,
            Name = "Test"
        };

        PurchaseLineItem lineItem = new PurchaseLineItem() {
            ItemEntity = itemEntity,
            Price = 10,
            Quantity = 5,
            Report = 5
        };

        PurchaseEntity purchaseEntity = new PurchaseEntity() {
            BillingParty = billingPartyEntity,
            Date = DateOnly.FromDateTime(DateTime.Now),
            InvoiceNumber = 12,
            PaidAmount = 10,
            Purchases = [lineItem],
            Remarks = "Test Remarks",
            TransportFee = 10,
            VatAmount = 10
        };

        double expectedBillingPartyBalance = billingPartyEntity.Balance +purchaseEntity.GetExtraAmount();
        double expectedItemStock = itemEntity.CurrentStockAmount + lineItem.Quantity;

        // Act- when adding a purchase
        AddPurchaseService.AddPurchase(purchaseEntity);

        // Assert

        // Billing party balance is updated
        Assert.Equal(expectedBillingPartyBalance, billingPartyEntity.Balance);
        // Item stock is updated
        Assert.Equal(expectedItemStock, itemEntity.CurrentStockAmount);
    }


    [Fact]
    public void AddPurchaseServiceWithMultipleLineItem_IncreasesStock_AndDecreasesBillingPartyBalance() {
        // Arrange
        ItemEntity itemEntity1 = new ItemEntity() {
            CurrentStockAmount = 10,
            CurrentEstimatedStockValuePerKilo = 10,
            Name = "test 1",
        };

        ItemEntity itemEntity2 = new ItemEntity() {
            CurrentStockAmount = 20,
            CurrentEstimatedStockValuePerKilo = 10,
            Name = "test 2",
        };

        BillingPartyEntity billingPartyEntity = new BillingPartyEntity() {
            Address = "Test" ,
            Balance = 10_000,
            Name = "Test"
        };

        PurchaseLineItem lineItem1 = new PurchaseLineItem() {
            ItemEntity = itemEntity1,
            Price = 10,
            Quantity = 5,
            Report = 5
        };

        PurchaseLineItem lineItem2 = new PurchaseLineItem() {
            ItemEntity = itemEntity2,
            Price = 10,
            Quantity = 5,
            Report = 5
        };

        PurchaseEntity purchaseEntity = new PurchaseEntity() {
            BillingParty = billingPartyEntity,
            Date = DateOnly.FromDateTime(DateTime.Now),
            InvoiceNumber = 12,
            PaidAmount = 10,
            Purchases = [lineItem1, lineItem2],
            Remarks = "Test Remarks",
            TransportFee = 10,
            VatAmount = 10
        };

        double expectedBillingPartyBalance =billingPartyEntity.Balance + purchaseEntity.GetExtraAmount();
        double expectedItemStock1 = itemEntity1.CurrentStockAmount + lineItem1.Quantity;
        double expectedItemStock2 = itemEntity2.CurrentStockAmount + lineItem2.Quantity;

        // Act- when adding a purchase
        AddPurchaseService.AddPurchase(purchaseEntity);

        // Assert

        // Billing party balance is updated
        Assert.Equal(expectedBillingPartyBalance, billingPartyEntity.Balance);
        // Item stock is updated
        Assert.Equal(expectedItemStock1, itemEntity1.CurrentStockAmount);
        Assert.Equal(expectedItemStock2, itemEntity2.CurrentStockAmount);
    }
}