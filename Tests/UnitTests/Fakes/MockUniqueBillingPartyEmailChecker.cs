using Application.services.billingParty;

namespace UnitTests.Fakes;

public class MockUniqueBillingPartyEmailChecker : IUniqueBillingPartyEmailChecker{

    public required bool Value { get; set; }
    public Task<bool> IsUniqueAsync(string email, Guid idToExclude = new Guid()) {
        return Task.FromResult(Value);
    }
}