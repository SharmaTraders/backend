using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.purchase;

public class PurchaseBillingItemTest
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

}