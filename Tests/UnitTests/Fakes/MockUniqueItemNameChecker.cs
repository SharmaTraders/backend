using Application.services.item;

namespace UnitTests.Fakes;

public class MockUniqueItemNameChecker: IUniqueItemNameChecker {
    public required bool Value { get; set; }

    public Task<bool> IsUniqueAsync(string name, Guid idToExclude = new Guid()) {
        return Task.FromResult(Value);
    }
}