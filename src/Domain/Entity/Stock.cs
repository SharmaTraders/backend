using Domain.Entity.ValueObjects;

namespace Domain.Entity;

public class Stock {
    private double _weight;
    private double _expectedValuePerKilo;
    private NonFutureDate _date;

    public Guid Id { get; set; }

    public required DateOnly Date {
        get => _date.Value;
        set => _date = new NonFutureDate(value);
    }

    public required double Weight {
        get => _weight;
        set {
            ValidateWeight(value);
            _weight = value;
        }
    }

    public double ExpectedValuePerKilo {
        get => _expectedValuePerKilo;
        set {
            ValidateValuePerKilo(value);
            _expectedValuePerKilo = value;
        }
    }


    private Remarks? _remarks;

    public string? Remarks {
        get => _remarks?.Value;
        set =>
            _remarks = value != null ? new Remarks(value) : null;
    }

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
}