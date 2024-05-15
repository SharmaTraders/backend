using System;
using System.Collections.Generic;

namespace Query;

public partial class Stock
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public double Weight { get; set; }

    public double ExpectedValuePerKilo { get; set; }

    public string? Remarks { get; set; }

    public string EntryCategory { get; set; } = null!;

    public Guid ItemEntityId { get; set; }

    public virtual Item ItemEntity { get; set; } = null!;
}
