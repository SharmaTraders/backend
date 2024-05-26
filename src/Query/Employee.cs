using System;
using System.Collections.Generic;

namespace Query;

public partial class Employee
{
    public Guid Id { get; set; }

    public double Balance { get; set; }

    public string Status { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public int NormalDailyWorkingMinute { get; set; }

    public virtual ICollection<EmployeeSalary> EmployeeSalaries { get; set; } = new List<EmployeeSalary>();

    public virtual ICollection<EmployeeWorkShift> EmployeeWorkShifts { get; set; } = new List<EmployeeWorkShift>();
}
