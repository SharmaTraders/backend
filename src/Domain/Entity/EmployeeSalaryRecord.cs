namespace Domain.Entity;

public class EmployeeSalary
{
    public Guid Id { get; set; }
    public required DateOnly FromDate { get; set; }
    private DateOnly? _toDate;
    private double _salaryPerHour;
    private double _overtimeSalaryPerHour;
    
    public required double SalaryPerHour
    {
        get => _salaryPerHour;
        set
        {
            ValidateSalary(value, nameof(SalaryPerHour));
            _salaryPerHour = value;
        }
    }
    
    public required double OvertimeSalaryPerHour
    {
        get => _overtimeSalaryPerHour;
        set
        {
            ValidateSalary(value, nameof(OvertimeSalaryPerHour));
            _overtimeSalaryPerHour = value;
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

    private static void ValidateSalary(double value, string propertyName)
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