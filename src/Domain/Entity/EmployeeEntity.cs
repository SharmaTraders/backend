using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Domain.common;

namespace Domain.Entity;

public class EmployeeEntity : IEntity<Guid> {
    public Guid Id { get; set; }
    private string _name;
    private string _address;
    private string? _email;
    private string? _phoneNumber;
    public double Balance { get; set; }
    public required EmployeeStatusCategory Status { get; set; }
    private int _normalDailyWorkingMinute;

    public ICollection<EmployeeWorkShift> WorkShifts { get; set; } = new List<EmployeeWorkShift>();

    public ICollection<EmployeeSalary> SalaryRecords { get; set; } = new List<EmployeeSalary>();

    public required string Name {
        get => _name;
        set {
            ValidateName(value);
            _name = value;
        }
    }

    public required string Address {
        get => _address;
        set {
            ValidateAddress(value);
            _address = value;
        }
    }

    public string? Email {
        get => _email;
        set {
            ValidateEmail(value);
            _email = string.IsNullOrEmpty(value) ? null : value;
        }
    }

    public string? PhoneNumber {
        get => _phoneNumber;
        set {
            ValidatePhoneNumber(value);
            _phoneNumber = string.IsNullOrEmpty(value) ? null : value;
        }
    }

    public required int NormalDailyWorkingMinute {
        get => _normalDailyWorkingMinute;
        set {
            ValidateNormalDailyWorkingMinute(value);
            _normalDailyWorkingMinute = value;
        }
    }

    public void AddBalance(double amount) {
        Balance += amount;
    }

    public void UpdateSalary(EmployeeSalary salary, [Optional] bool force) {
        if (!force) {
            bool salaryExists = SalaryRecords.Any(existingSalary => existingSalary.FromDate <= salary.FromDate
                                                                    && existingSalary.ToDate is not null &&
                                                                    existingSalary.ToDate >= salary.ToDate);
            if (salaryExists) {
                throw new DomainValidationException("SalaryRecordAlreadyExists", ErrorCode.Conflict,
                    ErrorMessages.SalaryRecordExists(salary.FromDate));
            }
        }

        EmployeeSalary? recordForDate = FindSalaryRecordForDate(salary.FromDate);
        if (recordForDate is not null) {
            if (Math.Abs(recordForDate.SalaryPerHour - salary.SalaryPerHour) < 0.01 &&
                Math.Abs(recordForDate.OvertimeSalaryPerHour - salary.OvertimeSalaryPerHour) < 0.01) {
                throw new DomainValidationException("SalaryRecord", ErrorCode.BadRequest,
                    ErrorMessages.SalaryRecordNoChange);
            }

            // End the previous salary record
            recordForDate.ToDate = salary.FromDate.AddDays(-1);
        }

        SalaryRecords.Add(salary);
    }

    public void AddTimeRecord(EmployeeWorkShift workShift) {
        CalculateAndApplyWorkShiftEarnings(workShift);
        WorkShifts.Add(workShift);
    }

    private void CalculateAndApplyWorkShiftEarnings(EmployeeWorkShift workShift) {
        // Find the salary record for the date of the work shift
        EmployeeSalary? salaryRecord = FindSalaryRecordForDate(workShift.Date);
        if (salaryRecord is null) {
            throw new DomainValidationException("SalaryRecord", ErrorCode.NotFound,
                ErrorMessages.SalaryRecordNotFound(workShift.Date));
        }

        // Convert the work shift start and end times to total minutes and subtract the break time
        double totalWorkMinutes = (workShift.EndTime.ToTimeSpan() - workShift.StartTime.ToTimeSpan()).TotalMinutes -
                                  workShift.BreakMinutes;

        // If the total work minutes exceed the normal daily working hours, calculate the overtime salary
        if (totalWorkMinutes > NormalDailyWorkingMinute) {
            var overtimeMinutes = totalWorkMinutes - NormalDailyWorkingMinute;
            Balance += (overtimeMinutes / 60) * salaryRecord.OvertimeSalaryPerHour;
            totalWorkMinutes -= overtimeMinutes;
        }

        Balance += Math.Round(((totalWorkMinutes / 60) * salaryRecord.SalaryPerHour), 2);
    }

    private EmployeeSalary? FindSalaryRecordForDate(DateOnly date) {
        return SalaryRecords.Where(record => record.FromDate <= date
                                             && (record.ToDate is null || record.ToDate >= date)
        ).MaxBy(record => record.FromDate);
    }

    private static void ValidateNormalDailyWorkingMinute(int value) {
        if (value is <= 0 or > 1440) {
            throw new DomainValidationException("NormalDailyWorkingMinute", ErrorCode.BadRequest,
                ErrorMessages.NormalDailyWorkHoursValidMinutes);
        }
    }

    private static void ValidateName(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("Name", ErrorCode.BadRequest, ErrorMessages.NameRequired);
        }

        // Length between 3 and 50 characters (inclusive)
        if (value.Length is < 3 or > 50) {
            throw new DomainValidationException("Name", ErrorCode.BadRequest,
                ErrorMessages.EmployeeFullNameBetween3And50);
        }
    }

    private static void ValidateAddress(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("Address", ErrorCode.BadRequest,
                ErrorMessages.AddressIsRequired("Employee"));
        }

        // Length between 3 and 60 characters (inclusive)
        if (value.Length is < 3 or > 60) {
            throw new DomainValidationException("Address", ErrorCode.BadRequest,
                ErrorMessages.AddressBetween3And60("Employee"));
        }
    }

    private static void ValidateEmail(string? value) {
        // Email is optional
        if (string.IsNullOrEmpty(value)) {
            return;
        }

        value = value.Trim();
        const string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(value);

        if (!match.Success) {
            throw new DomainValidationException("Email", ErrorCode.BadRequest, ErrorMessages.EmailInvalidFormat);
        }
    }

    private static void ValidatePhoneNumber(string? value) {
        // Phone number is optional
        if (string.IsNullOrEmpty(value)) {
            return;
        }

        if (!Regex.IsMatch(value, @"^\d+$")) {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.EmployeePhoneNumberMustBeAllDigits);
        }

        if (value.Length != 10) {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.PhoneNumberMustBe10DigitsLong);
        }
    }
}