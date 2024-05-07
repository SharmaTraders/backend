namespace Domain.utils;

public class DomainValidationException : Exception {
    public string Type { get; init; }
    public ErrorCode ErrorCode { get; init; }


    public DomainValidationException(string type, ErrorCode errorCode, string message) : base(message) {
        Type = type;
        ErrorCode = errorCode;
    }
}