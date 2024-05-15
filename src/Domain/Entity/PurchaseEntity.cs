using System.ComponentModel.DataAnnotations;
using Domain.common;

namespace Domain.Entity;

public class PurchaseEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorMessages.PurchaseEntityBillingPartyRequired)]
    public required BillingPartyEntity BillingParty { get; set; }

    private ICollection<PurchaseLineItem> _purchases;

    [Required(ErrorMessage = ErrorMessages.PurchaseEntityPurchaseLinesRequired)]
    public required ICollection<PurchaseLineItem> Purchases
    {
        get => _purchases;
        set
        {
            ValidatePurchases(value);
            _purchases = value;
        }
    }

    private double? _vatAmount;

    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseEntityVatAmountPositive)]
    public double? VatAmount
    {
        get => _vatAmount;
        set
        {
            ValidateTwoDecimalPlaces(value, "VatAmount", ErrorMessages.PurchaseEntityNumberRoundedToTwoDecimalPlaces);
            _vatAmount = value;
        }
    }

    private double? _transportFee;

    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseEntityTransportFeePositive)]
    public double? TransportFee
    {
        get => _transportFee;
        set
        {
            ValidateTwoDecimalPlaces(value, "TransportFee", ErrorMessages.PurchaseEntityNumberRoundedToTwoDecimalPlaces);
            _transportFee = value;
        }
    }

    private double? _paidAmount;

    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseEntityPaidAmountPositive)]
    public double? PaidAmount
    {
        get => _paidAmount;
        set
        {
            ValidateTwoDecimalPlaces(value, "PaidAmount", ErrorMessages.PurchaseEntityNumberRoundedToTwoDecimalPlaces);
            _paidAmount = value;
        }
    }

    private string? _remarks;

    [MaxLength(500, ErrorMessage = ErrorMessages.PurchaseEntityRemarksTooLong)]
    public string? Remarks
    {
        get => _remarks;
        set
        {
            ValidateRemarks(value);
            _remarks = value;
        }
    }

    private int? _invoiceNumber;

    [Range(0, int.MaxValue, ErrorMessage = ErrorMessages.PurchaseEntityInvoiceNumberPositive)]
    public int? InvoiceNumber
    {
        get => _invoiceNumber;
        set
        {
            ValidateInvoiceNumber(value);
            _invoiceNumber = value;
        }
    }

    private DateOnly _date;

    [Required(ErrorMessage = ErrorMessages.PurchaseEntityDateRequired)]
    public required DateOnly Date
    {
        get => _date;
        set
        {
            ValidateDate(value);
            _date = value;
        }
    }

    private void ValidateDate(DateOnly value)
    {
        if (value > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new DomainValidationException("Date", ErrorCode.BadRequest, ErrorMessages.DateCannotBeFutureDate);
        }
    }

    private void ValidateInvoiceNumber(int? value)
    {
        if (value is not null && value < 0)
        {
            throw new DomainValidationException("InvoiceNumber", ErrorCode.BadRequest, ErrorMessages.PurchaseEntityInvoiceNumberPositive);
        }
    }

    private void ValidateRemarks(string? value)
    {
        if (value is not null && value.Length > 500)
        {
            throw new DomainValidationException("Remarks", ErrorCode.BadRequest, ErrorMessages.PurchaseEntityRemarksTooLong);
        }
    }

    private void ValidatePurchases(ICollection<PurchaseLineItem> value)
    {
        if (value is null || value.Count == 0)
        {
            throw new DomainValidationException("Purchases", ErrorCode.BadRequest, ErrorMessages.PurchaseEntityPurchaseLinesRequired);
        }
    }

    private void ValidateTwoDecimalPlaces(double? value, string propertyName, string errorMessage)
    {
        if (!value.HasValue) return;

        if (value < 0)
        {
            throw new DomainValidationException(propertyName, ErrorCode.BadRequest, errorMessage);
        }

        double roundedValue = Math.Round(value.Value, 2);
        if (Math.Abs(roundedValue - value.Value) > 0.0001)
        {
            throw new DomainValidationException(propertyName, ErrorCode.BadRequest, errorMessage);
        }
    }
}
