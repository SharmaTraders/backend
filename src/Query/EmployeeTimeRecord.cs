using System;
using System.Collections.Generic;

namespace Query;

public partial class EmployeeTimeRecord
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public TimeSpan Break { get; set; }

    public Guid EmployeeEntityId { get; set; }

    public virtual Employee EmployeeEntity { get; set; } = null!;
}
