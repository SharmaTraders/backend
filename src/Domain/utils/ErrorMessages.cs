namespace Domain.utils;

public static class ErrorMessages {

    public const string InvalidEmailFormat = "Invalid email format";
    public const string EmailRequired = "Email is required";
    public const string PasswordRequired = "Password is required";
    public const string PasswordBiggerThan5Characters = "Password must be at least 6 characters long";

    public const string PasswordMustContainLetterAndNumber =
        "Password must contain at least one letter and one number";

    public const string EmailDoesntExist = "Email doesn't exist";
    public const string IncorrectPassword = "Incorrect password";
    public const string InvalidGuid = "Invalid id ";

    public const string AdminWithEmailAlreadyExists = "Admin with this email already exists";

    public const string ItemNameAlreadyExists = "Item with this name already exists";
    public const string ItemNameIsRequired = "Item must have a name";
    public const string ItemNameLength = "Item length must be between 3 and 20 (inclusive)";
}