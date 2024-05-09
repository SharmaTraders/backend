using System.Text.RegularExpressions;

namespace Domain.Entity;

public class BillingPartyEntity: IEntity<Guid> {
    private string _name;
    private string _address;
    private string? _email;
    private string? _phoneNumber;
    private double _balance;
    private string? _vatNumber;

    public Guid Id { get; set; }

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
            _email = value;
        }
    }

    public string? PhoneNumber {
        get => _phoneNumber;
        set {
            ValidatePhoneNumber(value);
            _phoneNumber = value;
        }
    }


    public double Balance {
        get => _balance;
        set {
            ValidateBalance(value);
            _balance = value;
        }
    }

    public string? VatNumber {
        get => _vatNumber;
        set {
            ValidateVatNumber(value);
            _vatNumber = value;
        }
    }


    private static void ValidateName(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("Name", ErrorCode.BadRequest, ErrorMessages.BillingPartyNameIsRequired);
        }

        //Length between 3 and 30 characters (inclusive)
        if (value.Length is < 3 or > 30) {
            throw new DomainValidationException("Name", ErrorCode.BadRequest, ErrorMessages.BillingPartyNameBetween3And30);
        }
    }

    private static void ValidateAddress(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("Address", ErrorCode.BadRequest, ErrorMessages.BillingPartyAddressIsRequired);
        }

        //Length between 3 and 30 characters (inclusive)
        if (value.Length is < 3 or > 60) {
            throw new DomainValidationException("Address", ErrorCode.BadRequest, ErrorMessages.BillingPartyAddressBetween3And60);
        }
    }

    private static void ValidateEmail(string? value) {
        // Email  is optional
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
                ErrorMessages.PhoneNumberMustBeAllDigits);
        }

        if (value.Length != 10) {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.PhoneNumberMustBe10DigitsLong);
        }
    }

    private static void ValidateBalance(double? value) {
        if (value is null) return;

        // Makes sure that upto two digits after decimal is acceptable
        string balanceStr = value.ToString()!;
        int decimalSeparatorIndex = balanceStr.IndexOf('.');
        if (decimalSeparatorIndex < 0) {
            return;
        }

        if (!(balanceStr.Length - decimalSeparatorIndex - 1 <= 2)) {
            throw new DomainValidationException("OpeningBalance", ErrorCode.BadRequest,
                ErrorMessages.OpeningBalanceMustBeAtMax2DecimalPlaces);
        }
    }

    private static void ValidateVatNumber(string? value) {
        // Vat number  is optional
        if (string.IsNullOrEmpty(value)) {
            return;
        }

        if (value.Length is < 5 or > 20) {
            throw new DomainValidationException("VatNumber", ErrorCode.BadRequest,
                ErrorMessages.VatNumberMustBeBetween5To20Characters);
        }
    }
    
}