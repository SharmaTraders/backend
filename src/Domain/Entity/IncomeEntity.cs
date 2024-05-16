using Domain.common;
using Domain.Entity.ValueObjects;

namespace Domain.Entity;

public class IncomeEntity : IEntity<Guid> {
    public Guid Id { get; set; }

    private NonFutureDate _date;

    public required DateOnly Date {
        get => _date.Value;
        set => _date = new NonFutureDate(value);
    }

    public required BillingPartyEntity BillingParty { get; set; }

    private Remarks? _remarks;
    public string? Remarks {
        get => _remarks?.Value;
        set => _remarks = value != null ? new Remarks(value) : null;
    }

    private double _amount;
    public double Amount {
        get => _amount;
        set {
            ValidateAmount(value);
            _amount = value;
        }
    }

    private static void ValidateAmount(double value) {
        if (value <= 0) {
            throw new DomainValidationException("Amount", ErrorCode.BadRequest,
                ErrorMessages.IncomeAmountMustBePositive);
        }

        double roundedValue = Math.Round(value, 2);
        if (Math.Abs(roundedValue - value) > 0.0001) {
            throw new DomainValidationException("Amount", ErrorCode.BadRequest,
                ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
        }
    }
}