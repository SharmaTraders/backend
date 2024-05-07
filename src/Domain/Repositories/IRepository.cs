using Domain.Entity;

namespace Domain.Repositories;

public interface IRepository<TEntity, in TId> where TEntity : IEntity<TId> {

    Task AddAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(TId id);
    
}