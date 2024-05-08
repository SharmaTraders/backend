using Dto;

namespace Domain.item;

public interface IItemDomain {
    Task CreateItem(CreateItemRequest createItemRequest);
    Task<ICollection<ItemDto>> GetAllItems();
    Task UpdateItem(string id, UpdateItemRequest updateItemRequest);
}