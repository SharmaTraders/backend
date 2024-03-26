using Domain.billingParty;
using Domain.utils;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class ValidatePhoneNumberTests {
    
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyPhoneNumber), MemberType = typeof(BillingPartyFactory))]

    public void ValidatePhoneNumber_WithValidBillingPartyPhoneNumber_Success(string validPhoneNumber) {
        // Arrange
        var billingPartyDomain = new BillingPartyDomain(null!);

        // Act                    
        billingPartyDomain.ValidatePhoneNumber(validPhoneNumber);
        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyPhoneNumber), MemberType = typeof(BillingPartyFactory))]

    public void ValidatePhoneNumber_WithInValidBillingPartyPhoneNumber_Fails(string invalidPhoneNumber) {
        // Arrange
        var billingPartyDomain = new BillingPartyDomain(null!);

        // Act and assert                    
        ValidationException exception = Assert.Throws<ValidationException>(() =>
            billingPartyDomain.ValidatePhoneNumber(invalidPhoneNumber));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
    }

}