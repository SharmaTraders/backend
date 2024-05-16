using Domain.Entity;
using Domain.Repository;

namespace Data.Repository;

public class IncomeRepository : IIncomeRepository {

    private readonly WriteDatabaseContext _context;

    public IncomeRepository(WriteDatabaseContext context) {
        _context = context;
    }

    public Task AddAsync(IncomeEntity entity) {
        return _context.Incomes.AddAsync(entity).AsTask();
    }

    public Task<IncomeEntity?> GetByIdAsync(Guid id) {
        return _context.Incomes.FindAsync(id).AsTask();
    }
}