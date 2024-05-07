using Domain.billingParty;
using Domain.utils;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class ValidateAddressTests {
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyAddress), MemberType = typeof(BillingPartyFactory))]
    public void ValidateAddress_WithValidBillingPartyAddress_Success(string validAddress) {
        // Arrange
        var billingPartyDomain = new BillingPartyDomain(null!, null!);

        // Act                    
        billingPartyDomain.ValidateAddress(validAddress);
        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyAddress), MemberType = typeof(BillingPartyFactory))]
    public void ValidateAddress_WithInValidBillingPartyAddress_Fails(string invalidAddress) {
        // Arrange
        var billingPartyDomain = new BillingPartyDomain(null!, null!);

        // Act and assert                    
        DomainValidationException exception = Assert.Throws<DomainValidationException>(() =>
            billingPartyDomain.ValidateAddress(invalidAddress));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
    }
}