using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class AdminRepository : IAdminRepository {
    private readonly WriteDatabaseContext _context;

    public AdminRepository(WriteDatabaseContext context) {
        _context = context;
    }


    public async Task AddAsync(AdminEntity entity) {
        await _context.Admins.AddAsync(entity);
    }

    public async Task<AdminEntity?> GetByIdAsync(Guid id) {
        return await _context.Admins.FindAsync(id);
    }

    public async Task<AdminEntity?> GetByEmailAsync(string email) {
        return await _context.Admins.FirstOrDefaultAsync(admin => admin.Email.ToLower().Equals(email.ToLower()));
    }
}