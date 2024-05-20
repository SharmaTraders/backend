using Domain.Entity;
using Domain.Repository;

namespace Data.Repository;

public class EmployeeRepository : IEmployeeRepository {
    private readonly WriteDatabaseContext _context;

    public EmployeeRepository(WriteDatabaseContext context) {
        _context = context;
    }

    public async Task AddAsync(EmployeeEntity entity) {
        await _context.Employees.AddAsync(entity);
    }
    
    public async Task<EmployeeEntity?> GetByIdAsync(Guid id) {
        return await _context.Employees.FindAsync(id);
    }
}