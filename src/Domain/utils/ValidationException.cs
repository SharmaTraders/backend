namespace Domain.utils;

public class ValidationException : Exception {
    public string Type { get; init; }
    public ErrorCode ErrorCode { get; init; }


    public ValidationException(string type, ErrorCode errorCode, string message) : base(message) {
        Type = type;
        ErrorCode = errorCode;
    }
}