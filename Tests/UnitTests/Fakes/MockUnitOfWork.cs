using Domain;

namespace UnitTests.Fakes;

public class MockUnitOfWork : IUnitOfWork {

    public int CallCount { get; set; } = 0;
    public Task SaveChangesAsync() {
        CallCount++;
        return Task.CompletedTask;
    }
}