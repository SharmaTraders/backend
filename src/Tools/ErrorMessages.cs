namespace Tools;

public static class ErrorMessages
{
    // Common
    public const string EmailInvalidFormat = "Invalid email format";
    public const string EmailRequired = "Email is required";

    public static string IdInvalid(string id) => $"Provided id : {id} is not a valid GUID";
    public const string DateFormatInvalid = "Invalid date format, The date must be in the format yyyy-MM-dd";


    // Admin
    public const string AdminEmailAlreadyExists = "Admin with this email already exists";
    public const string AdminEmailDoesntExist = "Email doesn't exist";
    public const string AdminPasswordRequired = "Password is required";
    public const string AdminPasswordBiggerThan5Characters = "Password must be at least 6 characters long";

    public const string AdminPasswordMustContainLetterAndNumber =
        "Password must contain at least one letter and one number";

    public const string AdminPasswordIncorrect = "Incorrect password";

    // Item
    public static string ItemNameAlreadyExists(string itemName) => $"Item name : {itemName} already exists";
    public const string ItemNameIsRequired = "Item must have a name";
    public const string ItemNameLength = "Item length must be between 3 and 20 (inclusive)";

    // Stock
    public const string StockValuePerKiloCannotBeNegative = "Stock value per kilo cannot be negative";
    public const string StockReduceCannotBeMoreThanCurrentStock = "Cannot reduce stock more than current stock";

    public const string StockAmountCannotBeNegative =
        "Stock weight cannot be negative, If you want to reduce stock use reduce stock endpoint";

    public const string StockAmountCannotBeZero = "Please provide a weight to add/reduce stock";
    public const string StockRemarksTooLong = "Remarks must be at most 500 characters long";


    // Billing parties
    public static string BillingPartyNameAlreadyExists(string billingPartyName) =>
        $"Billing party name : {billingPartyName} already exists";

    public static string BillingPartyEmailAlreadyExists(string email) =>
        $"Billing party with email : {email} already exists";

    public static string BillingPartyVatNumberAlreadyExists(string vatNumber) =>
        $"Billing party with vatNumber : {vatNumber} already exists";

    public const string BillingPartyNameIsRequired = "Billing party must have a name";

    public const string BillingPartyNameBetween3And30 =
        "Billing party name length must be between 3 and 30 (inclusive)";

    public const string BillingPartyAddressIsRequired = "Billing party address have a name";

    public const string BillingPartyAddressBetween3And60 =
        "Billing party address length must be between 3 and 60 (inclusive)";

    public const string BillingPartyPhoneNumberMustBeAllDigits = "Phone number must contain only digits";
    public const string BillingPartyPhoneNumberMustBe10DigitsLong = "Phone number must be 10 digits long";

    public const string BillingPartyOpeningBalanceMustBeAtMax2DecimalPlaces =
        "Opening balance must be at most 2 decimal places";

    public const string BillingPartyVatNumberMustBeBetween5To20Characters =
        "Vat number must be between 5 to 20 characters (inclusive)";

    // Purchase
    public const string PurchaseLineItemQuantityPositive = "Quantity must be positive or zero.";
    public const string PurchaseLineItemPricePositive = "Price must be positive or zero.";
    public const string PurchaseLineItemPriceRequired = "Price is required.";
    public const string PurchaseLineItemEntityRequired = "Item is required.";
    public const string PurchaseLineItemQuantityRequired = "Quantity is required.";
    public const string PurchaseLineItemReportPositive = "Report must be positive or zero.";
    public const string PurchaseEntityBillingPartyRequired = "Billing party is required.";
    public const string PurchaseEntityPurchaseLinesRequired = "At least one purchase line item is required.";
    public const string PurchaseEntityVatAmountPositive = "VAT amount must be a positive value.";
    public const string PurchaseEntityTransportFeePositive = "Transport fee must be a positive value.";
    public const string PurchaseEntityPaidAmountPositive = "Paid amount must be a positive value.";
    public const string PurchaseEntityRemarksTooLong = "Remarks cannot exceed 500 characters.";
    public const string PurchaseEntityInvoiceNumberPositive = "Invoice number must be a positive value.";
    public const string PurchaseEntityDateRequired = "Date is required.";
    public const string PurchaseEntityDateCannotBeFutureDate = "Future date cannot be assigned.";
    public const string PurchaseEntityDateFormatInvalid = "Invalid date format. Expected format is YYYY-MM-DD.";
    public const string PurchaseEntityInvalidPurchaseLineItem = "Invalid purchase line item.";


    public static string BillingPartyNotFound(Guid id)
    {
        return $"Billing party with id : {id} not found";
    }


    public static string ItemNotFound(Guid id)
    {
        return $"Item with id : {id} not found";
    }
}