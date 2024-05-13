namespace Domain.common;

public interface IUnitOfWork {
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}