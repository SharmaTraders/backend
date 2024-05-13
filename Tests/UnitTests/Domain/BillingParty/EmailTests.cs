using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class EmailTests {
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyEmails), MemberType = typeof(BillingPartyFactory))]

    public void BillingParty_WithValidBillingPartyEmail_CanBeCreated(string validEmail) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Email = validEmail,
            Address = "Valid Address",
            Name = "Test Name"
        };
    }  

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyEmails), MemberType = typeof(BillingPartyFactory))]

    public void BillingParty_WithInValidBillingPartyEmail_CannotBeCreated(string invalidEmail) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Email = invalidEmail,
            Name = "Test Name"
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("email", StringComparison.OrdinalIgnoreCase));

    }
}