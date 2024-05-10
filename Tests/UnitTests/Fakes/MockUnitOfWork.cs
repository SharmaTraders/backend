using Domain.common;

namespace UnitTests.Fakes;

public class MockUnitOfWork : IUnitOfWork {

    public int CallCount { get; set; } = 0;
    public Task SaveChangesAsync() {
        CallCount++;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) {
        CallCount++;
        return Task.CompletedTask;
    }
}