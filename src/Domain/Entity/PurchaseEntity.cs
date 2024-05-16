using Domain.common;
using Domain.Entity.ValueObjects;

namespace Domain.Entity;

public class PurchaseEntity : IEntity<Guid> {
    public Guid Id { get; set; }

    public required BillingPartyEntity BillingParty { get; set; }

    private ICollection<PurchaseLineItem> _purchases;

    public required ICollection<PurchaseLineItem> Purchases {
        get => _purchases;
        set {
            ValidatePurchases(value);
            _purchases = value;
        }
    }

    private double? _vatAmount;

    public double? VatAmount {
        get => _vatAmount;
        set {
            ValidateTwoDecimalPlaces(value, "VatAmount", ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
            _vatAmount = value;
        }
    }

    private double? _transportFee;

    public double? TransportFee {
        get => _transportFee;
        set {
            ValidateTwoDecimalPlaces(value, "TransportFee",
                ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
            _transportFee = value;
        }
    }

    private double? _paidAmount;

    public double? PaidAmount {
        get => _paidAmount;
        set {
            ValidateTwoDecimalPlaces(value, "PaidAmount", ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
            _paidAmount = value;
        }
    }


    private Remarks? _remarks;

    public string? Remarks {
        get => _remarks?.Value;
        set =>
            _remarks = value != null ? new Remarks(value) : null;
    }

    private int? _invoiceNumber;

    public int? InvoiceNumber {
        get => _invoiceNumber;
        set {
            ValidateInvoiceNumber(value);
            _invoiceNumber = value;
        }
    }

    private NonFutureDate _date;

    public required DateOnly Date {
        get => _date.Value;
        set =>
            _date = new NonFutureDate(value);
    }
    public double GetExtraAmount() {
        return (PaidAmount ?? 0) - GetTotalAmount();
    }

    // If needed can be made public later
    private double GetTotalAmount() {
        double totalAmount = Purchases.Sum(purchase => purchase.GetTotalAmount());
        totalAmount += TransportFee ?? 0;
        totalAmount += VatAmount ?? 0;
        return Math.Round(totalAmount, 2);
    }

    // Validation methods

    private static void ValidateInvoiceNumber(int? value) {
        if (value is not null && value < 0) {
            throw new DomainValidationException("InvoiceNumber", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityInvoiceNumberPositive);
        }
    }

    private static void ValidatePurchases(ICollection<PurchaseLineItem> value) {
        if (value is null || value.Count == 0) {
            throw new DomainValidationException("Purchases", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityPurchaseLinesRequired);
        }
    }

    private static void ValidateTwoDecimalPlaces(double? value, string propertyName, string errorMessage) {
        if (!value.HasValue) return;

        if (value < 0) {
            throw new DomainValidationException(propertyName, ErrorCode.BadRequest, errorMessage);
        }

        double roundedValue = Math.Round(value.Value, 2);
        if (Math.Abs(roundedValue - value.Value) > 0.0001) {
            throw new DomainValidationException(propertyName, ErrorCode.BadRequest, errorMessage);
        }
    }
}