using Domain.Entity;
using Domain.Repository;

namespace Data.Repository;

public class ExpenseRepository : IExpenseRepository {
    private readonly WriteDatabaseContext _context;

    public ExpenseRepository(WriteDatabaseContext context) {
        _context = context;
    }

    public Task AddAsync(ExpenseEntity entity) {
        return _context.Expenses.AddAsync(entity).AsTask();
    }

    public Task<ExpenseEntity?> GetByIdAsync(Guid id) {
        return _context.Expenses.FindAsync(id).AsTask();
    }
}