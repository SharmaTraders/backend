using System;
using System.Collections.Generic;

namespace Query;

public partial class Employee
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<EmployeeTimeRecord> EmployeeTimeRecords { get; set; } = new List<EmployeeTimeRecord>();
}
