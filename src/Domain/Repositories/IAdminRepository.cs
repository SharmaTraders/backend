using Domain.Entity;

namespace Domain.Repositories;

public interface IAdminRepository : IRepository<AdminEntity, Guid> {
    Task<AdminEntity?> GetByEmailAsync(string email);
    
}