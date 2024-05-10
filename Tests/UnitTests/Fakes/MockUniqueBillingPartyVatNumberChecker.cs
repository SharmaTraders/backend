using Application.services.billingParty;

namespace UnitTests.Fakes;

public class MockUniqueBillingVatNumberChecker : IUniqueBillingPartyVatNumberChecker{

    public required bool Value { get; set; }
    public Task<bool> IsUniqueAsync(string email, Guid idToExclude = new Guid()) {
        return Task.FromResult(Value);
    }
}