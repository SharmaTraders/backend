namespace Tools;

public static class ErrorMessages {
    // Common
    public const string EmailInvalidFormat = "Invalid email format";
    public const string EmailRequired = "Email is required";
    public const string RemarksTooLong = "Remarks must be at most 500 characters long";

    public static string IdInvalid(string id) => $"Provided id : {id} is not a valid GUID";
    public const string DateFormatInvalid = "Invalid date format, The date must be in the format yyyy-MM-dd";
    public const string DateCannotBeFutureDate = "Future date cannot be assigned.";
    public const string ValueMustBe2DecimalPlacesAtMax = "The value must be rounded to two decimal places or fewer.";
    public const string NameRequired = "Full name is required";
    public static string AddressIsRequired(string type) => $"{type} address is required";
    public static string AddressBetween3And60(string type) =>$"{type} address length must be between 3 and 60 (inclusive)";


    // Admin
    public const string AdminEmailAlreadyExists = "Admin with this email already exists";
    public const string AdminEmailDoesntExist = "Email doesn't exist";
    public const string AdminPasswordRequired = "Password is required";
    public const string AdminPasswordBiggerThan5Characters = "Password must be at least 6 characters long";

    public const string AdminPasswordMustContainLetterAndNumber =
        "Password must contain at least one letter and one number";

    public const string AdminPasswordIncorrect = "Incorrect password";

    // Item
    public static string ItemNotFound(Guid id) =>
        $"Item with id : {id} not found";


    public static string ItemNameAlreadyExists(string itemName) => $"Item name : {itemName} already exists";
    public const string ItemNameIsRequired = "Item must have a name";
    public const string ItemNameLength = "Item length must be between 3 and 20 (inclusive)";

    // Stock
    public const string StockValuePerKiloCannotBeNegative = "Stock value per kilo cannot be negative";
    public const string StockReduceCannotBeMoreThanCurrentStock = "Cannot reduce stock more than current stock";

    public const string StockAmountCannotBeNegative =
        "Stock weight cannot be negative, If you want to reduce stock use reduce stock endpoint";

    public const string StockAmountCannotBeZero = "Please provide a weight to add/reduce stock";


    // Billing parties
    public static string BillingPartyNotFound(Guid id) => $"Billing party with id : {id} not found";

    public static string BillingPartyNameAlreadyExists(string billingPartyName) =>
        $"Billing party name : {billingPartyName} already exists";

    public static string BillingPartyEmailAlreadyExists(string email) =>
        $"Billing party with email : {email} already exists";

    public static string BillingPartyVatNumberAlreadyExists(string vatNumber) =>
        $"Billing party with vatNumber : {vatNumber} already exists";

    public const string BillingPartyNameIsRequired = "Billing party must have a name";

    public const string BillingPartyNameBetween3And30 =
        "Billing party name length must be between 3 and 30 (inclusive)";
    
    public const string BillingPartyPhoneNumberMustBeAllDigits = "Phone number must contain only digits";
    public const string PhoneNumberMustBe10DigitsLong = "Phone number must be 10 digits long";

    public const string BillingPartyOpeningBalanceMustBeAtMax2DecimalPlaces =
        "Opening balance must be at most 2 decimal places";

    public const string BillingPartyVatNumberMustBeBetween5To20Characters =
        "Vat number must be between 5 to 20 characters (inclusive)";

    public const string BillingPartyUpdateAmountMustBeAtMax2DecimalPlaces =
        "Billing party update amount must be at most 2 decimal places";

    // Invoice-Item
    public const string InvoiceItemQuantityPositive = "The quantity must be a positive number.";
    public const string InvoiceItemPricePositive = "The price must be a positive number.";
    public const string InvoiceItemPriceRequired = "The price of the item is required.";
    public const string InvoiceItemEntityRequired = "The item is required and cannot be null.";
    public const string InvoiceItemQuantityRequired = "The quantity of the item is required.";
    public const string InvoiceItemReportPositive = "The report value must be a positive number.";

    // Invoice 
    public const string InvoiceBillingPartyRequired = "The billing party information is required.";
    public const string InvoiceItemLineRequired = "At least one item must be included.";
    public const string InvoiceVatAmountPositive = "The VAT amount must be a positive number.";
    public const string InvoiceTransportFeePositive = "The transport fee must be a positive number.";
    public const string PurchaseEntityPaidAmountPositive = "The paid amount must be a positive number.";
    public const string InvoiceNumberPositive = "The invoice number must be a positive integer.";
    public const string SaleEntityReceivedAmountPositive = "The received amount must be a positive number.";


    // IncomeEntity
    public const string IncomeAmountMustBePositive = "The income amount must be a positive number.";

    // ExpenseCategoryEntity
    public const string ExpenseCategoryNameRequired = "The expense category name is required.";
    public static string ExpenseCategoryNotFound(string name) => $"Expense category with name : {name} not found";

    // Expense
    public const string ExpenseCategoryNameBetween2And100 =
        "The expense category name must be between 2 and 100 characters long (inclusive).";
    public const string ExpenseCategoryNameAlreadyExists = "The expense category name already exists.";
    public const string ExpenseEitherCategoryOrBillingPartyRequired =
        "One of category or billing party is required";

    // Employee
    public const string EmployeeFullNameBetween3And50 = "Full name length must be between 3 and 50 (inclusive)";
    public const string EmployeePhoneNumberMustBeAllDigits = "Phone number must contain only digits";
    public const string EmployeeStatusInvalid = "Employee status must be either 'active' or 'inactive' or 'terminated'";
    public const string EmployeeStatusIsRequired = "Employee status is required";

    public static string EmployeeEmailAlreadyExists(string employeeEntityEmail) =>
        $"Employee with email : {employeeEntityEmail} already exists";

    public static string EmployeePhoneNumberAlreadyExists(string employeeEntityPhoneNumber) =>
        $"Employee with phone number : {employeeEntityPhoneNumber} already exists";
    
    public static string EmployeeNotFound(Guid guid) => $"Employee with id : {guid} not found";

}