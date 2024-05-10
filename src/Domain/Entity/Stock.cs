namespace Domain.Entity;

public class Stock {
    public Guid Id { get; set; }
    public required DateOnly Date { get; set; }
    public required double Weight { get; set; }
    public  double ExpectedValuePerKilo { get; set; }
    public required StockEntryCategory EntryCategory { get; set; }

}