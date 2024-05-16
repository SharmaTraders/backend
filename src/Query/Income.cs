using System;
using System.Collections.Generic;

namespace Query;

public partial class Income
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public Guid BillingPartyId { get; set; }

    public string? Remarks { get; set; }

    public double Amount { get; set; }

    public virtual BillingParty BillingParty { get; set; } = null!;
}
