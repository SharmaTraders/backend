using System.Text.RegularExpressions;
using Domain.common;

namespace Domain.Entity;

public class EmployeeEntity: IEntity<Guid> {
    private string _fullName;
    private string _address;
    private string? _email;
    private string? _phoneNumber;
    private string _status;

    public Guid Id { get; set; }

    public required string FullName {
        get => _fullName;

        set {
            ValidateFullName(value);
            _fullName = value;
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

    public required string Status {
        get => _status;
        set {
            ValidateStatus(value);
            _status = value;
        }
    }

    public ICollection<EmployeeTimeRecord> TimeRecords { get; set; } = new List<EmployeeTimeRecord>();

    
    public void AddTimeRecord(EmployeeTimeRecord timeRecord) {
        TimeRecords.Add(timeRecord);
    }
    
    private static void ValidateFullName(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("FullName", ErrorCode.BadRequest, ErrorMessages.NameRequired);
        }

        // Length between 3 and 50 characters (inclusive)
        if (value.Length is < 3 or > 50) {
            throw new DomainValidationException("FullName", ErrorCode.BadRequest, ErrorMessages.EmployeeFullNameBetween3And50);
        }
    }

    private static void ValidateAddress(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("Address", ErrorCode.BadRequest, ErrorMessages.AddressIsRequired("Employee"));
        }

        // Length between 3 and 60 characters (inclusive)
        if (value.Length is < 3 or > 60)
        {
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
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest, ErrorMessages.EmployeePhoneNumberMustBeAllDigits);
        }

        if (value.Length != 10) {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest, ErrorMessages.PhoneNumberMustBe10DigitsLong);
        }
    }

    private static void ValidateStatus(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("Status", ErrorCode.BadRequest, ErrorMessages.EmployeeStatusIsRequired);
        }

        // Status can only be "Active", "Inactive" or "Terminated"
        string[] validStatuses = ["Active", "Inactive", "Terminated"];
        if (!validStatuses.Contains(value)) {
            throw new DomainValidationException("Status", ErrorCode.BadRequest, ErrorMessages.EmployeeStatusInvalid);
        }
    }
}
