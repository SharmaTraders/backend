using Domain.Entity;

namespace Domain.Repository;

public interface IItemRepository : IRepository<ItemEntity, Guid> {

    Task<ItemEntity?> GetByNameAsync(string name);

    Task<List<ItemEntity>> GetAllAsync();
}