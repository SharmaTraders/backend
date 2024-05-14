using System.ComponentModel.DataAnnotations;
using Domain.common;

namespace Domain.Entity;

public class PurchaseEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorMessages.PurchaseEntityBillingPartyRequired)]
    public BillingPartyEntity BillingParty { get; set; }

    private ICollection<PurchaseLineItem> _purchases;

    [Required(ErrorMessage = ErrorMessages.PurchaseEntityPurchaseLinesRequired)]
    public ICollection<PurchaseLineItem> Purchases
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
            ValidateVatAmount(value);
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
            ValidateTransportFee(value);
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
            ValidatePaidAmount(value);
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

    private void ValidateRemarks(string? value)
    {
        if (value != null && value.Length > 500)
        {
            throw new DomainValidationException("Remarks", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityRemarksTooLong);
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
    public DateOnly Date
    {
        get => _date;
        set
        {
            ValidateDate(value);
            _date = value;
        }
    }
    
    private void ValidatePurchases(ICollection<PurchaseLineItem> value)
    {
        foreach (var item in value)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(item);
            if (!Validator.TryValidateObject(item, context, validationResults, true))
            {
                throw new DomainValidationException("PurchaseLineItem", ErrorCode.BadRequest,
                    ErrorMessages.PurchaseEntityInvalidPurchaseLineItem);
            }
        }
    }

    private void ValidateVatAmount(double? value)
    {
        if (!value.HasValue) return;
        if (value < 0)
        {
            throw new DomainValidationException("VatAmount", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityVatAmountPositive);
        }

        // Check if value is rounded to two decimal places
        double roundedValue = Math.Round(value.Value, 2);
        if (Math.Abs(roundedValue - value.Value) >
            0.0001) // Allowing a small tolerance for floating-point arithmetic
        {
            throw new DomainValidationException("VatAmount", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityNumberRoundedToTwoDecimalPlaces);
        }
    }
    
    private void ValidatePaidAmount(double? value)
    {
        if (!value.HasValue) return;
        if (value < 0)
        {
            throw new DomainValidationException("PaidAmount", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityPaidAmountPositive);
        }

        // Check if value is rounded to two decimal places
        double roundedValue = Math.Round(value.Value, 2);
        if (Math.Abs(roundedValue - value.Value) >
            0.0001) // Allowing a small tolerance for floating-point arithmetic
        {
            throw new DomainValidationException("PaidAmount", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityNumberRoundedToTwoDecimalPlaces);
        }
    }
    
    private void ValidateInvoiceNumber(int? value)
    {
        if (!value.HasValue) return;

        if (value < 0)
        {
            throw new DomainValidationException("InvoiceNumber", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityInvoiceNumberPositive);
        }
    }
    
    private void ValidateTransportFee(double? value)
    {
        if (!value.HasValue) return;
        if (value < 0)
        {
            throw new DomainValidationException("TransportFee", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityTransportFeePositive);
        }

        // Check if value is rounded to two decimal places
        double roundedValue = Math.Round(value.Value, 2);
        if (Math.Abs(roundedValue - value.Value) >
            0.0001) // Allowing a small tolerance for floating-point arithmetic
        {
            throw new DomainValidationException("TransportFee", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityNumberRoundedToTwoDecimalPlaces);
        }
    }

    private void ValidateDate(DateOnly value)
    {
        var dateString = value.ToString("yyyy-MM-dd");
        if (!DateOnly.TryParse(dateString, out _))
        {
            throw new DomainValidationException("Date", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityDateFormatInvalid);
        }

        if (value > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new DomainValidationException("Date", ErrorCode.BadRequest,
                ErrorMessages.PurchaseEntityDateCannotBeFutureDate);
        }
    }
}