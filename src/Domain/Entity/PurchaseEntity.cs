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
                foreach (var item in value)
                {
                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(item);
                    if (!Validator.TryValidateObject(item, context, validationResults, true))
                    {
                        throw new DomainValidationException("PurchaseLineItem", ErrorCode.BadRequest, ErrorMessages.PurchaseEntityInvalidPurchaseLineItem);
                    }
                }
            _purchases = value;
        }
    }


    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseEntityVatAmountPositive)]
    public double? VatAmount { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseEntityTransportFeePositive)]
    public double? TransportFee { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseEntityPaidAmountPositive)]
    public double PaidAmount { get; set; }

    [MaxLength(500, ErrorMessage = ErrorMessages.PurchaseEntityRemarksTooLong)]
    public string? Remarks { get; set; }

    private int? _invoiceNumber;

    [Range(0, int.MaxValue, ErrorMessage = ErrorMessages.PurchaseEntityInvoiceNumberPositive)]
    public int? InvoiceNumber
    {
        get => _invoiceNumber;
        set
        {
            if (value < 0)
            {
                throw new DomainValidationException("InvoiceNumber", ErrorCode.BadRequest, ErrorMessages.PurchaseEntityInvoiceNumberPositive);
            }
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
            var dateString = value.ToString("yyyy-MM-dd");
            if (!DateOnly.TryParse(dateString, out _))
            {
                throw new DomainValidationException("Date", ErrorCode.BadRequest, ErrorMessages.PurchaseEntityDateFormatInvalid);
            }
            if (value > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new DomainValidationException("Date", ErrorCode.BadRequest, ErrorMessages.PurchaseEntityDateCannotBeFutureDate);
            }
            _date = value;
        }
    }

}