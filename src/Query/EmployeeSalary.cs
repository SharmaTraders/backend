using System;
using System.Collections.Generic;

namespace Query;

public partial class EmployeeSalary
{
    public Guid Id { get; set; }

    public DateOnly FromDate { get; set; }

    public double SalaryPerHour { get; set; }

    public double OvertimeSalaryPerHour { get; set; }

    public DateOnly? ToDate { get; set; }

    public Guid EmployeeEntityId { get; set; }

    public virtual Employee EmployeeEntity { get; set; } = null!;
}
