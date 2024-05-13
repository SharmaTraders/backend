using System;
using System.Collections.Generic;

namespace Query;

public partial class PurchaseLineItem
{
    public Guid Id { get; set; }

    public Guid ItemEntityId { get; set; }

    public double Quantity { get; set; }

    public double Price { get; set; }

    public double Report { get; set; }

    public Guid PurchaseEntityId { get; set; }

    public virtual Item ItemEntity { get; set; } = null!;

    public virtual Purchase PurchaseEntity { get; set; } = null!;
}
