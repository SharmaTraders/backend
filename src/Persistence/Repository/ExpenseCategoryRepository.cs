using Domain.Entity;
using Domain.Repository;

namespace Data.Repository;

public class ExpenseCategoryRepository : IExpenseCategoryRepository {

    private readonly WriteDatabaseContext _context;

    public ExpenseCategoryRepository(WriteDatabaseContext context) {
        _context = context;
    }

    public Task AddAsync(ExpenseCategoryEntity entity) {
        return _context.ExpenseCategories.AddAsync(entity).AsTask();
    }

    public Task<ExpenseCategoryEntity?> GetByIdAsync(string id) {
        return _context.ExpenseCategories.FindAsync(id).AsTask();
    }
}