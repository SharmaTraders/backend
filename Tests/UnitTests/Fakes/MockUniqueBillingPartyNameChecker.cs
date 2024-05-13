using Application.services.billingParty;

namespace UnitTests.Fakes;

public class MockUniqueBillingPartyNameChecker : IUniqueBillingPartyNameChecker{

    public required bool Value { get; set; }
    public Task<bool> IsUniqueAsync(string name, Guid idToExclude = new Guid()) {
        return Task.FromResult(Value);
    }
}