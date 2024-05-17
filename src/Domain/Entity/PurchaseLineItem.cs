using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class PurchaseLineItem
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorMessages.InvoiceItemLineRequired)]
    public required ItemEntity ItemEntity { get; set; }

    private double _quantity;

    [Required(ErrorMessage = ErrorMessages.InvoiceItemQuantityRequired)]
    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.InvoiceItemQuantityPositive)]
    public required double Quantity
    {
        get => _quantity;
        set
        {
            ValidateQuantity(value);
            _quantity = value;
        }
    }

    private double _price;

    [Required(ErrorMessage = ErrorMessages.InvoiceItemPriceRequired)]
    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.InvoiceItemPricePositive)]
    public required double Price
    {
        get => _price;
        set
        {
            ValidatePrice(value);
            _price = value;
        }
    }

    private double? _report;

    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.InvoiceItemReportPositive)]
    public double? Report
    {
        get => _report;
        set
        {
            ValidateReport(value);
            _report = value;
        }
    }
    
    public double GetTotalAmount()
    {
        return (Quantity * Price) - (Report ?? 0);
    }

    private static void ValidateTwoDecimalPlaces(double value, string type)
    {
        double roundedValue = Math.Round(value, 2);
        if (Math.Abs(roundedValue - value) > 0.0001)
        {
            throw new DomainValidationException(type, ErrorCode.BadRequest, ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
        }
    }

    private static void ValidateIsMoreThanZero(double value, string property, string errorMessage)
    {
        if (value <= 0)
        {
            throw new DomainValidationException(property, ErrorCode.BadRequest, errorMessage);
        }
    }

    private static void ValidateIsPositiveNumber(double value, string property, string errorMessage)
    {
        if (value < 0)
        {
            throw new DomainValidationException(property, ErrorCode.BadRequest, errorMessage);
        }
    }

    private static void ValidateQuantity(double value)
    {
        ValidateTwoDecimalPlaces(value, "Quantity");
        ValidateIsMoreThanZero(value, "Quantity", ErrorMessages.InvoiceItemQuantityPositive);
    }

    private static void ValidatePrice(double value)
    {
        ValidateTwoDecimalPlaces(value, "Price");
        ValidateIsMoreThanZero(value, "Price", ErrorMessages.InvoiceItemPricePositive);
    }

    private static void ValidateReport(double? value)
    {
        if (!value.HasValue) return;
        ValidateTwoDecimalPlaces(value.Value, "Report");
        ValidateIsPositiveNumber(value.Value, "Report", ErrorMessages.InvoiceItemReportPositive);
    }
}
