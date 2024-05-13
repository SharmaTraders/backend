using Domain.common;
using Domain.Entity;

namespace Domain.Repository;

public interface IRepository<TEntity, in TId> where TEntity : IEntity<TId> {

    Task AddAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(TId id);
    
}