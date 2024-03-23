using Dto;

namespace Domain.dao;

public interface IItemDao {
    Task<ItemDto?> GetItemByName(string itemDtoName);
    Task CreateItem(ItemDto itemDto);
}