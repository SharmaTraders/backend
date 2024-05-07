using Dto;

namespace Domain.item;

public interface IItemDomain {
    Task CreateItem(AddItemRequest addItemRequest);
    Task<ICollection<ItemDto>> GetAllItems();
}