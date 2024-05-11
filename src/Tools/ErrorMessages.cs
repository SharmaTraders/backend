namespace Tools;

public static class ErrorMessages {

    public const string EmailInvalidFormat = "Invalid email format";
    public const string EmailRequired = "Email is required";
    public const string EmailAlreadyExists = "Admin with this email already exists";
    public const string EmailDoesntExist = "Email doesn't exist";


    public const string PasswordRequired = "Password is required";
    public const string PasswordBiggerThan5Characters = "Password must be at least 6 characters long";
    public const string PasswordMustContainLetterAndNumber =
        "Password must contain at least one letter and one number";
    public const string PasswordIncorrect = "Incorrect password";
    public const string StockValuePerKiloCannotBeNegative = "Stock value per kilo cannot be negative";


    public static string IdInvalid(string id) => $"Provided id : {id} is not a valid GUID";

    public static string ItemNameAlreadyExists(string itemName) => $"Item name : {itemName} already exists";
    public const string ItemNameIsRequired = "Item must have a name";
    public const string ItemNameLength = "Item length must be between 3 and 20 (inclusive)";

    public static string BillingPartyNameAlreadyExists(string billingPartyName) => $"Billing party name : {billingPartyName} already exists";
    public static string BillingPartyEmailAlreadyExists(string email) => $"Billing party with email : {email} already exists";
    public static string BillingPartyVatNumberAlreadyExists(string vatNumber) => $"Billing party with vatNumber : {vatNumber} already exists";
    public const string BillingPartyNameIsRequired = "Billing party must have a name";
    public const string BillingPartyNameBetween3And30 = "Billing party name length must be between 3 and 30 (inclusive)";


    public const string BillingPartyAddressIsRequired = "Billing party address have a name";
    public const string BillingPartyAddressBetween3And60 = "Billing party address length must be between 3 and 60 (inclusive)";

    public const string PhoneNumberMustBeAllDigits = "Phone number must contain only digits";
    public const string PhoneNumberMustBe10DigitsLong = "Phone number must be 10 digits long";

    public const string OpeningBalanceMustBePositive = "Opening balance must be positive";
    public const string OpeningBalanceMustBeAtMax2DecimalPlaces = "Opening balance must be at most 2 decimal places";

    public const string VatNumberMustBeBetween5To20Characters = "Vat number must be between 5 to 20 characters (inclusive)";
    public const string DateFormatInvalid = "Invalid date format, The date must be in the format yyyy-MM-dd";

    public const string StockAmountCannotBeNegative = "Stock amount cannot be negative";
    public const string StockReduceCannotBeMoreThanCurrentStock = "Cannot reduce stock more than current stock";
    public const string ValuePerKiloCannotBeNegative = "Value per kilo cannot be negative";

    public const string StockWeightCannotBeNegative = "Stock weight cannot be negative, If you want to reduce stock use reduce stock endpoint";
    public const string StockAmountCannotBeZero = "Please provide a weight to add/reduce stock";

    public static string BillingPartyNotFound(Guid id) {
        return $"Billing party with id : {id} not found";
    }


    public static string ItemNotFound(Guid id)
    {
        return $"Item with id : {id} not found";
    }
}