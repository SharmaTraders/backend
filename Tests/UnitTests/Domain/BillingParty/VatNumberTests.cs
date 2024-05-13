using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class VatNumberTests {
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyVatNumber), MemberType = typeof(BillingPartyFactory))]

    public void BillingParty_WithValidBillingPartyVatNumber_CanBeCreated(string validVatNumber) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Address = "Valid Address",
            Name = "validName" ,
            VatNumber = validVatNumber
        };
        Assert.Equal(validVatNumber, billingPartyEntity.VatNumber);
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyVatNumber), MemberType = typeof(BillingPartyFactory))]

    public void BillingParty_WithInValidBillingPartyVatNumber_CannotBeCreated(string invalidVatNumber) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Name = "valid name",
            VatNumber = invalidVatNumber
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("vatnumber", StringComparison.OrdinalIgnoreCase));
    }
}