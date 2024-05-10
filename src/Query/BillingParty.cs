using System;
using System.Collections.Generic;

namespace Query;

public partial class BillingParty
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public double Balance { get; set; }

    public string? VatNumber { get; set; }
}
