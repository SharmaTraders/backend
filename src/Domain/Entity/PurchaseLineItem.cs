﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class PurchaseLineItem
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorMessages.PurchaseLineItemEntityRequired)]
    public ItemEntity ItemEntity { get; set; }
    
    private double _quantity;
    
    [Required(ErrorMessage = ErrorMessages.PurchaseLineItemQuantityRequired)]
    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseLineItemQuantityPositive)]
    public double Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0)
            {
                throw new DomainValidationException("Quantity", ErrorCode.BadRequest, ErrorMessages.PurchaseLineItemQuantityPositive);
            }

            _quantity = value;
        }
    }
    
    private double _price;

    [Required(ErrorMessage = ErrorMessages.PurchaseLineItemPriceRequired)]
    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseLineItemPricePositive)]
    public double Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new DomainValidationException("Price", ErrorCode.BadRequest,
                    ErrorMessages.PurchaseEntityInvoiceNumberPositive);
            }

            _price = value;
        }
    }

    private double? _report;
    
    [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.PurchaseLineItemReportPositive)]
    public double? Report
    {
        get => _report;
        set
        {
            if (value < 0)
            {
                throw new DomainValidationException("Report", ErrorCode.BadRequest,
                    ErrorMessages.PurchaseLineItemReportPositive);
            }
            _report = value;
        }
    }
}