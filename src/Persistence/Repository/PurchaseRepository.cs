using Domain.Entity;
using Domain.Repository;

namespace Data.Repository;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly WriteDatabaseContext _context;

    public PurchaseRepository(WriteDatabaseContext context)
    {
        _context = context;
    }

    public Task AddAsync(PurchaseEntity entity)
    {
        return _context.AddAsync(entity).AsTask();
    }

    public Task<PurchaseEntity?> GetByIdAsync(Guid id)
    {
        return _context.Purchases.FindAsync(id).AsTask();
    }
}