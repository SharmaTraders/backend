using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.income;

public class RemarksTests {
    [Theory]
    [MemberData(nameof(RemarksFactory.GetValidRemarks), MemberType = typeof(RemarksFactory))]
    public void Income_WithValidRemarks_CanBeCreated(string validRemarks) {
        // No exception is thrown
        // No exception is thrown
        IncomeEntity entity = new IncomeEntity() {
            Amount = 100,
            BillingParty = null!,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Remarks = validRemarks
        };
        Assert.Equal(validRemarks, entity.Remarks);
    }

    [Theory]
    [MemberData(nameof(RemarksFactory.GetInvalidRemarks), MemberType = typeof(RemarksFactory))]
    public void Income_WithInValidRemarks_CannotBeCreated(string invalidRemarks) {
        // No exception is thrown
        var exception = Assert.Throws<DomainValidationException>( () => new IncomeEntity() {
            Amount = 100,
            BillingParty = null!,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Remarks = invalidRemarks
        });
        Assert.Equal("remarks", exception.Type.ToLower());
    }
}