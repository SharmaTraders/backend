using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class Address {
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyAddress), MemberType = typeof(BillingPartyFactory))]
    public void BillingParty_WithValidBillingPartyAddress_CanBeCreated(string validAddress) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Address = validAddress,
            Name = "Test Name"
        };
        Assert.Equal(validAddress, billingPartyEntity.Address);
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyAddress), MemberType = typeof(BillingPartyFactory))]
    public void BillingParty_WithInValidBillingPartyAddress_CannotBeCreated(string invalidAddress) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = invalidAddress,
            Name = "Test Name"
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("address", StringComparison.OrdinalIgnoreCase));


    } 
}