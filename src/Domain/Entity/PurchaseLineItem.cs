using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class PurchaseLineItem
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorMessages.PurchaseLineItemEntityRequired)]
    public required ItemEntity ItemEntity { get; set; }

    private double _quantity;

    [Required(ErrorMessage = ErrorMessages.PurchaseLineItemQuantityRequired)]
    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseLineItemQuantityPositive)]
    public required double Quantity
    {
        get => _quantity;
        set => _quantity = ValidateQuantity(value);
    }

    private double _price;

    [Required(ErrorMessage = ErrorMessages.PurchaseLineItemPriceRequired)]
    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseLineItemPricePositive)]
    public required double Price
    {
        get => _price;
        set => _price = ValidatePrice(value);
    }

    private double? _report;

    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseLineItemReportPositive)]
    public double? Report
    {
        get => _report;
        set => _report = ValidateReport(value);
    }

    private double ValidateQuantity(double value)
    {
        if (value <= 0)
        {
            throw new DomainValidationException("Quantity", ErrorCode.BadRequest, ErrorMessages.PurchaseLineItemQuantityPositive);
        }
        double roundedValue = Math.Round(value, 2);
        if (Math.Abs(roundedValue - value) > 0.0001)
        {
            throw new DomainValidationException("Quantity", ErrorCode.BadRequest, ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
        }
        return value;
    }

    private double ValidatePrice(double value)
    {
        if (value <= 0)
        {
            throw new DomainValidationException("Price", ErrorCode.BadRequest, ErrorMessages.PurchaseLineItemPricePositive);
        }
        double roundedValue = Math.Round(value, 2);
        if (Math.Abs(roundedValue - value) > 0.0001)
        {
            throw new DomainValidationException("Price", ErrorCode.BadRequest, ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
        }
        return value;
    }

    private double? ValidateReport(double? value)
    {
        if (!value.HasValue) return value;
        if (value < 0)
        {
            throw new DomainValidationException("Report", ErrorCode.BadRequest, ErrorMessages.PurchaseLineItemReportPositive);
        }
        double roundedValue = Math.Round(value.Value, 2);
        if (Math.Abs(roundedValue - value.Value) > 0.0001)
        {
            throw new DomainValidationException("Report", ErrorCode.BadRequest, ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
        }
        return value;
    }

    public double GetTotalAmount()
    {
        return (Quantity * Price - (Report ?? 0));
    }
}
