using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class PhoneNumberTests {
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyPhoneNumber), MemberType = typeof(BillingPartyFactory))]

    public void BillingParty_WithValidBillingPartyPhoneNumber_CanBeCreated(string validPhoneNumber) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Address = "Valid Address",
            Name = "validName" ,
            PhoneNumber = validPhoneNumber
        };
        Assert.Equal(string.IsNullOrEmpty(validPhoneNumber) ? null : validPhoneNumber, billingPartyEntity.PhoneNumber);
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyPhoneNumber), MemberType = typeof(BillingPartyFactory))]

    public void BillingParty_WithInValidBillingPartyPhoneNumber_CannotBeCreated(string invalidPhoneNumber) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Name = "validName",
            PhoneNumber = invalidPhoneNumber
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("phoneNumber", StringComparison.OrdinalIgnoreCase));
    } 
}