using System.ComponentModel.DataAnnotations;
using Domain.common;
using Domain.Entity.ValueObjects;

namespace Domain.Entity;

public class SaleEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    [Required] public required BillingPartyEntity BillingParty { get; set; }
    
    private NonFutureDate _date;
    public required DateOnly Date {
        get => _date.Value;
        set => _date = new NonFutureDate(value);
    }
    
    private ICollection<SaleLineItem> _sales;
    public required ICollection<SaleLineItem> Sales
    {
        get => _sales;
        set
        {
            ValidateSales(value);
            _sales = value;
        }
    }

    private double? _vatAmount;
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
    public double? TransportFee
    {
        get => _transportFee;
        set
        {
            ValidateTransportFees(value);
            _transportFee = value;
        }
    }
    
    private double? _receivedAmount;
    public double? ReceivedAmount
    {
        get => _receivedAmount;
        set
        {
            ValidateReceivedAmount(value);
            _receivedAmount = value;
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

    public double GetExtraAmount() {
        return GetTotalAmount() - (ReceivedAmount ?? 0);
    }

    private double GetTotalAmount() {
        double totalAmount = Sales.Sum(item => item.GetTotalAmount());
        totalAmount += TransportFee ?? 0;
        totalAmount += VatAmount ?? 0;
        return Math.Round(totalAmount, 2);
    }
    
    private static void ValidateTwoDecimalPlaces(double value, string propertyName)
    {
        double roundedValue = Math.Round(value, 2);
        if (Math.Abs(roundedValue - value) > 0.0001)
        {
            throw new DomainValidationException(propertyName, ErrorCode.BadRequest, ErrorMessages.ValueMustBe2DecimalPlacesAtMax);
        }
    }
    
    private static void ValidateIsPositiveNumber(double value, string propertyName, string errorMessage)
    {
        if (value < 0)
        {
            throw new DomainValidationException(propertyName, ErrorCode.BadRequest,
                errorMessage);
        }
    }
    
    private static void ValidateReceivedAmount(double? value)
    {
        if (!value.HasValue) return;
        ValidateTwoDecimalPlaces(value.Value, "ReceivedAmount");
        ValidateIsPositiveNumber(value.Value, "ReceivedAmount", ErrorMessages.SaleEntityReceivedAmountPositive);
    }
    

    private static void ValidateTransportFees(double? value)
    {
        if (!value.HasValue) return;
        ValidateTwoDecimalPlaces(value.Value, "TransportFee");
        ValidateIsPositiveNumber(value.Value, "TransportFee", ErrorMessages.InvoiceTransportFeePositive);
    }


    private static void ValidateVatAmount(double? value)
    {
        if (!value.HasValue) return;
        ValidateTwoDecimalPlaces(value.Value, "VatAmount");
        ValidateIsPositiveNumber(value.Value, "VatAmount", ErrorMessages.InvoiceVatAmountPositive);
    }

    private static void ValidateInvoiceNumber(int? value)
    {
        if (!value.HasValue) return;
        ValidateIsPositiveNumber(value.Value, "InvoiceNumber", ErrorMessages.InvoiceNumberPositive);
    }
    
    private static void ValidateSales(ICollection<SaleLineItem> value)
    {
        if (value is null || value.Count == 0)
        {
            throw new DomainValidationException("Sales", ErrorCode.BadRequest, ErrorMessages.InvoiceItemLineRequired);
        }
    }
}