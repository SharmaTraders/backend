using System;
using System.Collections.Generic;

namespace Query;

public partial class EmployeeSalaryRecord
{
    public Guid Id { get; set; }

    public DateOnly FromDate { get; set; }

    public double SalaryPerHr { get; set; }

    public double OvertimeSalaryPerHr { get; set; }

    public DateOnly? ToDate { get; set; }

    public Guid EmployeeEntityId { get; set; }

    public virtual Employee EmployeeEntity { get; set; } = null!;
}
