using Domain.Entity;

namespace Domain.Repositories;

public interface IItemRepository : IRepository<ItemEntity, Guid> {

    Task<ItemEntity?> GetByNameAsync(string name);

    Task<List<ItemEntity>> GetAllAsync();
}