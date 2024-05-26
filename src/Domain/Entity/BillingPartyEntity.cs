using System.Text.RegularExpressions;
using Domain.common;

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


    public double Balance {
        get => Math.Round(_balance);
        set {
            ValidateBalance(value);
            _balance = value;
        }
    }

    public string? VatNumber {
        get => _vatNumber;
        set {
            ValidateVatNumber(value);
            _vatNumber = string.IsNullOrEmpty(value) ? null : value;
        }
    }

    public void AddBalance(double amount) {
        ValidateBalanceForAdd(amount);
        _balance += Math.Round(amount, 2);
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
            throw new DomainValidationException("Address", ErrorCode.BadRequest, ErrorMessages.AddressIsRequired("Billing Party"));
        }

        //Length between 3 and 30 characters (inclusive)
        if (value.Length is < 3 or > 60) {
            throw new DomainValidationException("Address", ErrorCode.BadRequest, ErrorMessages.AddressBetween3And60("Billing Party"));
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
                ErrorMessages.BillingPartyPhoneNumberMustBeAllDigits);
        }

        if (value.Length != 10) {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.PhoneNumberMustBe10DigitsLong);
        }
    }

    private static void ValidateBalance(double? value) {
        if (value is null) return;
        
        double roundedValue = Math.Round(value.Value, 2);
        if (Math.Abs(roundedValue - value.Value) > 0.0001) {
            throw new DomainValidationException("OpeningBalance", ErrorCode.BadRequest,
                ErrorMessages.BillingPartyOpeningBalanceMustBeAtMax2DecimalPlaces);
        }
    }
    private void ValidateBalanceForAdd(double value)
    {
       double roundedValue = Math.Round(value, 2);
        if (Math.Abs(roundedValue - value) > 0.0001) {
            throw new DomainValidationException("Amount", ErrorCode.BadRequest,
                ErrorMessages.BillingPartyUpdateAmountMustBeAtMax2DecimalPlaces);
        }
    }

    private static void ValidateVatNumber(string? value) {
        // Vat number  is optional
        if (string.IsNullOrEmpty(value)) {
            return;
        }

        if (value.Length is < 5 or > 20) {
            throw new DomainValidationException("VatNumber", ErrorCode.BadRequest,
                ErrorMessages.BillingPartyVatNumberMustBeBetween5To20Characters);
        }
    }
    
}