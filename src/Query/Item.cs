using System;
using System.Collections.Generic;

namespace Query;

public partial class Item
{
    public Guid Id { get; set; }

    public double CurrentStockAmount { get; set; }

    public double CurrentEstimatedStockValuePerKilo { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
