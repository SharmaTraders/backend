using Domain.Entity;
using Domain.Repository;

namespace Data.Repository;

public class SaleRepository: ISaleRepository
{
    private readonly WriteDatabaseContext _context;

    public SaleRepository(WriteDatabaseContext context)
    {
        _context = context;
    }
    
    public Task AddAsync(SaleEntity entity)
    {
        return _context.AddAsync(entity).AsTask();
    }

    public Task<SaleEntity?> GetByIdAsync(Guid id)
    {
        return _context.Sales.FindAsync(id).AsTask();
    }
}