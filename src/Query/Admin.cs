using System;
using System.Collections.Generic;

namespace Query;

public partial class Admin
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
