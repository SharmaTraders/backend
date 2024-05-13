using System.ComponentModel.DataAnnotations;
using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.purchase;

public class PurchaseBillingItem
{
    [Theory]
    [MemberData(nameof(PurchaseFactory.GetValidPurchaseLineItems), MemberType = typeof(PurchaseFactory))]
    public void Purchase_WithValidPurchaseLineItem_CanBeCreated(PurchaseLineItem validPurchaseLineItem)
    {
        // Arrange
        var purchaseEntity = new PurchaseEntity
        {
            Id = Guid.NewGuid(),
            BillingParty = ValidObjects.GetValidBillingParty(),
            Date = DateOnly.FromDateTime(DateTime.Now),
            PaidAmount = 0,
            Purchases = new List<PurchaseLineItem>() { validPurchaseLineItem },
            TransportFee = 0,
            VatAmount = 0,
            InvoiceNumber = 0,
            Remarks = "Test Remarks"
        };

        // Act No exception is thrown
        Assert.Contains(validPurchaseLineItem, purchaseEntity.Purchases);
    }

    //  TODO: Implement the following tests 
    // [Theory]
    // [MemberData(nameof(PurchaseFactory.GetInvalidPurchaseLineItems), MemberType = typeof(PurchaseFactory))]
    // public void Purchase_WithInvalidPurchaseLineItem_CannotBeCreated(PurchaseLineItem invalidPurchaseLineItem)
    // {
    //     // Arrange & Act
    //     var exception = Record.Exception(() => new PurchaseEntity
    //     {
    //         Id = Guid.NewGuid(),
    //         BillingParty = ValidObjects.GetValidBillingParty(),
    //         Date = DateOnly.FromDateTime(DateTime.Now),
    //         PaidAmount = 0,
    //         Purchases = new List<PurchaseLineItem>() { invalidPurchaseLineItem },
    //         TransportFee = 0,
    //         VatAmount = 0,
    //         InvoiceNumber = 0,
    //         Remarks = "Test Remarks"
    //     });
    //
    //     // Assert
    //     Assert.IsType<DomainValidationException>(exception);
    //     Assert.NotNull(exception);
    //     Assert.Contains("Quantity must be positive or zero", exception.Message);
    // }


}