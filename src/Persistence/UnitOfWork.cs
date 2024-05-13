using Domain.common;

namespace Data;

public class UnitOfWork : IUnitOfWork {

    private readonly WriteDatabaseContext _context;

    public UnitOfWork(WriteDatabaseContext context) {
        _context = context;
    }

    public Task SaveChangesAsync() {
       return _context.SaveChangesAsync();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) {
        return _context.SaveChangesAsync(cancellationToken);
    }
}