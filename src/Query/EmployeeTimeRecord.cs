using System;
using System.Collections.Generic;

namespace Query;

public partial class EmployeeTimeRecord
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int BreakMinutes { get; set; }

    public Guid EmployeeEntityId { get; set; }

    public virtual Employee EmployeeEntity { get; set; } = null!;
}
