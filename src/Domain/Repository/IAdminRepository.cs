using Domain.Entity;

namespace Domain.Repository;

public interface IAdminRepository : IRepository<AdminEntity, Guid> {
    Task<AdminEntity?> GetByEmailAsync(string email);
    
}