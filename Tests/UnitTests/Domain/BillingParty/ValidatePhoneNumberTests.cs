using Domain.billingParty;
using Domain.Entity;
using Domain.utils;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class ValidatePhoneNumberTests {
    
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyPhoneNumber), MemberType = typeof(BillingPartyFactory))]

    public void ValidatePhoneNumber_WithValidBillingPartyPhoneNumber_Success(string validPhoneNumber) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Address = "Valid Address",
            Name = "validName" ,
            PhoneNumber = validPhoneNumber
        };
        Assert.Equal(validPhoneNumber, billingPartyEntity.PhoneNumber);
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyPhoneNumber), MemberType = typeof(BillingPartyFactory))]

    public void ValidatePhoneNumber_WithInValidBillingPartyPhoneNumber_Fails(string invalidPhoneNumber) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Name = "validName",
            PhoneNumber = invalidPhoneNumber
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("phoneNumber", StringComparison.OrdinalIgnoreCase));
    }

}