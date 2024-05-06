using Dto;

namespace Domain.dao;

public interface IItemDao {
    Task<AddItemRequest?> GetItemByName(string itemDtoName);
    Task CreateItem(AddItemRequest addItemRequest);
}