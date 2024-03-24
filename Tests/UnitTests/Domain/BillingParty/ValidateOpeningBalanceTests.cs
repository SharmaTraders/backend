using Domain.billingParty;
using Domain.utils;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class ValidateOpeningBalanceTests {

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyOpeningBalance), MemberType = typeof(BillingPartyFactory))]

    public void ValidateOpeningBalance_WithValidOpeningBalance_Success(double validOpeningBalance) {
        // Arrange
        var billingPartyDomain = new BillingPartyDomain(null);

        // Act
        billingPartyDomain.ValidateOpeningBalance(validOpeningBalance);

        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyOpeningBalance), MemberType = typeof(BillingPartyFactory))]

    public void ValidateOpeningBalance_WithInValidOpeningBalance_Fails(double invalidOpeningBalance) {
        // Arrange
        var billingPartyDomain = new BillingPartyDomain(null);

        // Act and assert
        ValidationException exception = Assert.Throws<ValidationException>(() =>
            billingPartyDomain.ValidateOpeningBalance(invalidOpeningBalance));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
    }


    
}