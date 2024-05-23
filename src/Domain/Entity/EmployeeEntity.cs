using System.Text.RegularExpressions;
using Domain.common;

namespace Domain.Entity;

public class EmployeeEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    private string _name;
    private string _address;
    private string? _email;
    private string? _phoneNumber;
    public double Balance { get; set; }
    public required EmployeeStatusCategory Status { get; set; }
    private TimeOnly _normalDailyWorkingHours;

    public ICollection<EmployeeWorkShift> WorkShifts { get; set; } = new List<EmployeeWorkShift>();

    public void AddTimeRecord(EmployeeWorkShift workShift)
    {
        CalculateAndApplyWorkShiftEarnings(workShift);
        WorkShifts.Add(workShift);
    }

    private void CalculateAndApplyWorkShiftEarnings(EmployeeWorkShift workShift)
    {
        // Find the salary record for the date of the work shift
        EmployeeSalaryRecord? salaryRecord = FindSalaryRecordForDate(workShift.Date);
        if (salaryRecord is null)
        {
            throw new DomainValidationException("SalaryRecord", ErrorCode.NotFound,
                ErrorMessages.SalaryRecordNotFound(workShift.Date));
        }
        
        // Convert the work shift start and end times to total minutes and subtract the break time
        var totalWorkMinutes = (workShift.EndTime.ToTimeSpan() - workShift.StartTime.ToTimeSpan()).TotalMinutes -
                               workShift.BreakMinutes;
        
        // If the total work minutes exceed the normal daily working hours, calculate the overtime salary
        if (totalWorkMinutes > NormalDailyWorkingHours.ToTimeSpan().TotalMinutes)
        {
            var overtimeMinutes = totalWorkMinutes - NormalDailyWorkingHours.ToTimeSpan().TotalMinutes;
            Balance += (overtimeMinutes/60) * salaryRecord.OvertimeSalaryPerHr;
            totalWorkMinutes -= overtimeMinutes;
        }
        Balance += Math.Round(((totalWorkMinutes / 60) * salaryRecord.SalaryPerHr), 2);
    }

    private EmployeeSalaryRecord? FindSalaryRecordForDate(DateOnly workShiftDate)
    {
        return SalaryRecords.Where( record => record.FromDate <= workShiftDate 
                                              && (record.ToDate is null || record.ToDate >= workShiftDate)
        ).MaxBy(record => record.FromDate);
    }

    public ICollection<EmployeeSalaryRecord> SalaryRecords { get; set; } = new List<EmployeeSalaryRecord>();

    public void AddSalaryRecord(EmployeeSalaryRecord salaryRecord)
    {
        SalaryRecords.Add(salaryRecord);
    }

    public required string Name
    {
        get => _name;
        set
        {
            ValidateName(value);
            _name = value;
        }
    }

    public required string Address
    {
        get => _address;
        set
        {
            ValidateAddress(value);
            _address = value;
        }
    }

    public string? Email
    {
        get => _email;
        set
        {
            ValidateEmail(value);
            _email = string.IsNullOrEmpty(value) ? null : value;
        }
    }

    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            ValidatePhoneNumber(value);
            _phoneNumber = string.IsNullOrEmpty(value) ? null : value;
        }
    }

    public required TimeOnly NormalDailyWorkingHours
    {
        get => _normalDailyWorkingHours;
        set
        {
            ValidateNormalDailyWorkHours(value);
            _normalDailyWorkingHours = value;
        }
    }

    private static void ValidateNormalDailyWorkHours(TimeOnly value)
    {
        if (value.ToTimeSpan().TotalMinutes <= 0 || value.ToTimeSpan().TotalMinutes > 1440)
        {
            throw new DomainValidationException("NormalDailyWorkingHours", ErrorCode.BadRequest,
                ErrorMessages.NormalDailyWorkHoursValidMinutes);
        }
    }

    private static void ValidateName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainValidationException("Name", ErrorCode.BadRequest, ErrorMessages.NameRequired);
        }

        // Length between 3 and 50 characters (inclusive)
        if (value.Length is < 3 or > 50)
        {
            throw new DomainValidationException("Name", ErrorCode.BadRequest,
                ErrorMessages.EmployeeFullNameBetween3And50);
        }
    }
    private bool HasOverlappingShift(EmployeeWorkShift newShift)
    {
        foreach (var existingShift in WorkShifts)
        {
            if (existingShift.Date == newShift.Date && 
                ((newShift.StartTime < existingShift.EndTime && newShift.EndTime > existingShift.StartTime) || 
                 (existingShift.StartTime < newShift.EndTime && existingShift.EndTime > newShift.StartTime)))
            {
                return true;
            }
        }
        return false;
    }
    

    private static void ValidateAddress(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainValidationException("Address", ErrorCode.BadRequest,
                ErrorMessages.AddressIsRequired("Employee"));
        }

        // Length between 3 and 60 characters (inclusive)
        if (value.Length is < 3 or > 60)
        {
            throw new DomainValidationException("Address", ErrorCode.BadRequest,
                ErrorMessages.AddressBetween3And60("Employee"));
        }
    }

    private static void ValidateEmail(string? value)
    {
        // Email is optional
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        value = value.Trim();
        const string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(value);

        if (!match.Success)
        {
            throw new DomainValidationException("Email", ErrorCode.BadRequest, ErrorMessages.EmailInvalidFormat);
        }
    }

    private static void ValidatePhoneNumber(string? value)
    {
        // Phone number is optional
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (!Regex.IsMatch(value, @"^\d+$"))
        {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.EmployeePhoneNumberMustBeAllDigits);
        }

        if (value.Length != 10)
        {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.PhoneNumberMustBe10DigitsLong);
        }
    }
}