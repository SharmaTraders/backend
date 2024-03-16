using Dto;

namespace Domain.dao;

public interface IItemDao {
    Task<ItemDto?> GetItemByName(string itemDtoName);
    Task<ItemDto> CreateItem(ItemDto itemDto);
}