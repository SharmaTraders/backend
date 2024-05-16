using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.income;

public  class AmountTests {

    [Theory]
    [MemberData(nameof(IncomeFactory.GetValidAmounts), MemberType = typeof(IncomeFactory))]
    public void Income_WithValidAmount_CanBeCreated(double validAmount) {
        // No exception is thrown
        IncomeEntity entity = new IncomeEntity() {
            Amount = validAmount,
            BillingParty = null!,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Remarks = ""
        };
        Assert.Equal(validAmount, entity.Amount);
    }

    [Theory]
    [MemberData(nameof(IncomeFactory.GetInvalidAmounts), MemberType = typeof(IncomeFactory))]
    public void Income_WithInValidAmount_CannotBeCreated(double invalidAmount) {
        // No exception is thrown
        var exception = Assert.Throws<DomainValidationException>( () => new IncomeEntity() {
            Amount = invalidAmount,
            BillingParty = null!,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Remarks = ""
        });
        Assert.Equal("amount", exception.Type.ToLower());
    }
    
}