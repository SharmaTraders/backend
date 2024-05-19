using Domain.Entity;

namespace Domain.DomainServices;

public static class AddPurchaseService {
    public static void AddPurchase(PurchaseEntity purchaseEntity ) {

        // Update balance of billing party
        double amountForBillingParty = purchaseEntity.GetExtraAmount();
        purchaseEntity.BillingParty.AddBalance(amountForBillingParty);


        // Add stock for each item in purchase
        foreach (PurchaseLineItem lineItem in purchaseEntity.Purchases) {
            ItemEntity itemEntity = lineItem.ItemEntity;
            Stock stock = new Stock() {
                Date = purchaseEntity.Date,
                EntryCategory = StockEntryCategory.Purchase,
                ExpectedValuePerKilo = lineItem.Price,
                Remarks = "Added from purchase entry.",
                Weight = lineItem.Quantity
            };
            itemEntity.AddStock(stock);
        }
    }
    
}