using System;
using System.Collections.Generic;

namespace Query;

public partial class Purchase
{
    public Guid Id { get; set; }

    public Guid BillingPartyId { get; set; }

    public double? VatAmount { get; set; }

    public double? TransportFee { get; set; }

    public double PaidAmount { get; set; }

    public string? Remarks { get; set; }

    public int? InvoiceNumber { get; set; }

    public DateOnly Date { get; set; }

    public virtual BillingParty BillingParty { get; set; } = null!;

    public virtual ICollection<PurchaseLineItem> PurchaseLineItems { get; set; } = new List<PurchaseLineItem>();
}
