using Domain.Entity;

namespace Domain.DomainServices;

public class AddSaleService
{
    public static void AddSale(SaleEntity saleEntity)
    {
        // Update balance of billing party
        double amountForBillingParty = saleEntity.GetExtraAmount();
        saleEntity.BillingParty.AddBalance(amountForBillingParty);

        // Remove stock for each item in sale
        foreach (SaleLineItem lineItem in saleEntity.Sales)
        {
            ItemEntity itemEntity = lineItem.ItemEntity;
            Stock stock = new Stock()
            {
                Date = saleEntity.Date,
                EntryCategory = StockEntryCategory.Sale,
                ExpectedValuePerKilo = lineItem.Price,
                Remarks = "Removed from sale entry to " + saleEntity.BillingParty.Name + ".",
                Weight = lineItem.Quantity 
            };
            itemEntity.ReduceStock(stock);
        }
    }
}