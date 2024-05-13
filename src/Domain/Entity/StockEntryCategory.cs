using System.Reflection;
using Domain.common;

namespace Domain.Entity;

public class StockEntryCategory : Enumeration{

    public static readonly StockEntryCategory Purchase= new StockEntryCategory(1, "Purchase");
    public static readonly StockEntryCategory Sale= new StockEntryCategory(2, "Sale");
    public static readonly StockEntryCategory AddStockEntry= new StockEntryCategory(3, "Add Stock");
    public static readonly StockEntryCategory RemoveStockEntry= new StockEntryCategory(4, "Remove Stock");


    private StockEntryCategory(){}

    private StockEntryCategory(int id, string name) : base(id, name) {
    }

    public static IEnumerable<StockEntryCategory> GetAll() {
        var fields =
            typeof(StockEntryCategory).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields) {
            var instance = new StockEntryCategory();

            if (info.GetValue(instance) is StockEntryCategory locatedValue) {
                yield return locatedValue;
            }
        }
    }
}