using System;
using System.Collections.Generic;

namespace Query;

public partial class Sale
{
    public Guid Id { get; set; }

    public Guid BillingPartyId { get; set; }

    public DateOnly Date { get; set; }

    public double? VatAmount { get; set; }

    public double? TransportFee { get; set; }

    public double? ReceivedAmount { get; set; }

    public string? Remarks { get; set; }

    public int? InvoiceNumber { get; set; }

    public virtual BillingParty BillingParty { get; set; } = null!;

    public virtual ICollection<SaleLineItem> SaleLineItems { get; set; } = new List<SaleLineItem>();
}
