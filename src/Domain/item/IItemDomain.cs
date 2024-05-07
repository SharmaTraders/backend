using Dto;

namespace Domain.item;

public interface IItemDomain {
    Task CreateItem(CreateItemRequest createItemRequest);
    Task<ICollection<ItemDto>> GetAllItems();
}