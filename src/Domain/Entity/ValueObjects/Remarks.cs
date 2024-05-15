namespace Domain.Entity.ValueObjects;

public class Remarks {
    private string _value;

    public string Value {
        get => _value;
        set {
            ValidateRemarks(value);
            _value = value;

        }
    }

    public Remarks(string value) {
        Value = value.Trim();
    }

    private static void ValidateRemarks(string value) {
        if (value.Length > 500) {
            throw new DomainValidationException("remarks", ErrorCode.BadRequest, ErrorMessages.RemarksTooLong);
            
        }
    }

    
}