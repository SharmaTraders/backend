using System.Runtime.InteropServices;
using Domain.Entity;

namespace Domain.Repository;

public interface IItemRepository : IRepository<ItemEntity, Guid> {

}