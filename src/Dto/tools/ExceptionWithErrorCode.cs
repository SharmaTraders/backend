namespace Dto.tools;

public class ExceptionWithErrorCode : Exception {

    public ErrorCode ErrorCode { get; set; }

    public ExceptionWithErrorCode(ErrorCode errorCode, string message) : base(message) {
        ErrorCode = errorCode;
    }
    
}