namespace Dto.tools;

public class ExceptionWithErrorCode(ErrorCode errorCode, string message) : Exception(message) {

    public ErrorCode ErrorCode { get; set; } = errorCode;
}