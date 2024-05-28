namespace Domain.common;

public interface IUnitOfWork {
    Task SaveChangesAsync(CancellationToken cancellationToken);
}