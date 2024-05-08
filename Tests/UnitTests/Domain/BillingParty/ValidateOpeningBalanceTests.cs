using Domain.billingParty;
using Domain.Entity;
using Domain.utils;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class ValidateOpeningBalanceTests {

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyOpeningBalance), MemberType = typeof(BillingPartyFactory))]

    public void ValidateOpeningBalance_WithValidOpeningBalance_Success(double validOpeningBalance) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Address = "Valid Address",
            Name = "validName" ,
            Balance = validOpeningBalance
        };
        Assert.Equal(validOpeningBalance, billingPartyEntity.Balance);
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyOpeningBalance), MemberType = typeof(BillingPartyFactory))]

    public void ValidateOpeningBalance_WithInValidOpeningBalance_Fails(double invalidOpeningBalance) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Name = "validName",
            Balance = invalidOpeningBalance
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("openingbalance", StringComparison.OrdinalIgnoreCase));
    }


    
}