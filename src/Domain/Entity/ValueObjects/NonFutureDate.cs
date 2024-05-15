namespace Domain.Entity.ValueObjects;

internal class NonFutureDate {
    private DateOnly _value;

    public DateOnly Value {
        get => _value;
        set {
            ValidateDate(value);
            _value = value;
        }
    }   

    public NonFutureDate(DateOnly value) {
        Value = value;
    }


    private static void ValidateDate(DateOnly value) {
        if (value > DateOnly.FromDateTime(DateTime.Now)) {
            throw new DomainValidationException("date", ErrorCode.BadRequest, ErrorMessages.DateCannotBeFutureDate);
        }
    }
}