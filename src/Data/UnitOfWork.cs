using Domain;

namespace Data;

public class UnitOfWork : IUnitOfWork {

    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context) {
        _context = context;
    }

    public Task SaveChangesAsync() {
       return _context.SaveChangesAsync();
    }
}