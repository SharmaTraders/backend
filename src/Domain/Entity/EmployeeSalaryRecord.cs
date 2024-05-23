using Domain.common;
using Domain.Entity.ValueObjects;

namespace Domain.Entity;

public class EmployeeSalaryRecord : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required DateOnly FromDate { get; set; }
    private DateOnly? _toDate;
    private double _salaryPerHr;
    private double _overtimeSalaryPerHr;
    
    public required double SalaryPerHr
    {
        get => _salaryPerHr;
        set
        {
            ValidaSalary(value, nameof(SalaryPerHr));
            _salaryPerHr = value;
        }
    }
    
    public required double OvertimeSalaryPerHr
    {
        get => _overtimeSalaryPerHr;
        set
        {
            ValidaSalary(value, nameof(OvertimeSalaryPerHr));
            _overtimeSalaryPerHr = value;
        }
    }

    public DateOnly? ToDate
    {
        get => _toDate;
        set
        {
            ValidateToDate(value);
            _toDate = value;
        }
    }

    private static void ValidaSalary(double value, string propertyName)
    {
        if (value < 0)
        {
            throw new DomainValidationException(propertyName, ErrorCode.BadRequest, ErrorMessages.SalaryPerHrInvalid(propertyName));
        }
    }
    
    private void ValidateToDate(DateOnly? value)
    {
        if (value.HasValue && value.Value < FromDate)
        {
            throw new DomainValidationException("ToDate", ErrorCode.BadRequest, ErrorMessages.ToDateBeforeFromDate);
        }
    }
}