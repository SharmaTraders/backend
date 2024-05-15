namespace Domain.Entity;

public class Stock {
    private double _weight;
    private double _expectedValuePerKilo;
    private string? _remarks;
    private DateOnly _date;

    public Guid Id { get; set; }
    public required DateOnly Date { get => _date;
        set {
            ValidateDate(value);
            _date = value;
        }}

    public required double Weight {
        get => _weight;
        set {
            ValidateWeight(value);
            _weight = value;
        }
    }

    public double ExpectedValuePerKilo { get => _expectedValuePerKilo;
        set {
            ValidateValuePerKilo(value);
            _expectedValuePerKilo = value;
        } }

    public string? Remarks { get => _remarks;
        set {
            ValidateRemarks(value);
            _remarks = value;
        } } 

    public required StockEntryCategory EntryCategory { get; set; }

    private static void ValidateWeight(double value) {
        if (value < 0) {
            throw new DomainValidationException("weight", ErrorCode.BadRequest,
                ErrorMessages.StockAmountCannotBeNegative);
        }

        if (value == 0) {
            throw new DomainValidationException("weight", ErrorCode.BadRequest, ErrorMessages.StockAmountCannotBeZero);
        }
    }

    private static void ValidateValuePerKilo(double value) {
        if (value < 0) {
            throw new DomainValidationException("expectedValuePerKilo", ErrorCode.BadRequest,
                ErrorMessages.StockValuePerKiloCannotBeNegative);
        }
    }

    private static void ValidateRemarks(string? value) {
        if (string.IsNullOrEmpty(value)) {
            return;
        }

        if (value.Length > 500) {
            throw new DomainValidationException("remarks", ErrorCode.BadRequest, ErrorMessages.StockRemarksTooLong);
            
        }
    }

    private static void ValidateDate(DateOnly value) {
        if (value > DateOnly.FromDateTime(DateTime.Now)) {
            throw new DomainValidationException("date", ErrorCode.BadRequest, ErrorMessages.DateCannotBeFutureDate);
        }
    }
}