using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class NameTests {

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public void BillingParty_WithValidBillingPartyName_CanBeCreated(string validName) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Address = "Valid Address",
            Name = validName
        };
        Assert.Equal(validName, billingPartyEntity.Name);
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public void BillingParty_WithInValidBillingPartyName_CannotBeCreated(string invalidName) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Name = invalidName
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("name", StringComparison.OrdinalIgnoreCase));
    }
    
}