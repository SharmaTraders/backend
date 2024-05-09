using Dto;

namespace DomainEntry.item;

public interface IItemDomain {
    Task CreateItem(CreateItemRequest createItemRequest);
    Task<ICollection<ItemDto>> GetAllItems();
    Task UpdateItem(string id, UpdateItemRequest updateItemRequest);
}