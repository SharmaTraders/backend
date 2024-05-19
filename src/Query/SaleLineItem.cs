using System;
using System.Collections.Generic;

namespace Query;

public partial class SaleLineItem
{
    public Guid Id { get; set; }

    public Guid ItemEntityId { get; set; }

    public double Quantity { get; set; }

    public double Price { get; set; }

    public double? Report { get; set; }

    public Guid SaleEntityId { get; set; }

    public virtual Item ItemEntity { get; set; } = null!;

    public virtual Sale SaleEntity { get; set; } = null!;
}
