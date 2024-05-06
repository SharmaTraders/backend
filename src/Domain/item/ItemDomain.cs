using Domain.dao;
using Domain.utils;
using Dto;

namespace Domain.item;

public class ItemDomain : IItemDomain {

    private readonly IItemDao _itemDao;

    public ItemDomain(IItemDao itemDao) {
        _itemDao = itemDao;
    }

    public async Task CreateItem(AddItemRequest addItemRequest) {
        addItemRequest = new AddItemRequest(addItemRequest.Name.Trim());
        ValidateItem(addItemRequest);

        AddItemRequest? itemFromDb = await _itemDao.GetItemByName(addItemRequest.Name);
        if (itemFromDb is not null) {
            throw new ValidationException("ItemName",ErrorCode.Conflict, ErrorMessages.ItemNameAlreadyExists(addItemRequest.Name)); 
        } 
        await _itemDao.CreateItem(addItemRequest);
    }

    private static void ValidateItem(AddItemRequest addItemRequest) {
        if (string.IsNullOrEmpty(addItemRequest.Name)) {
            throw new ValidationException("ItemName",ErrorCode.BadRequest, ErrorMessages.ItemNameIsRequired);
        }

        if (addItemRequest.Name.Length is < 3 or > 20) {
            throw new ValidationException("ItemName",ErrorCode.BadRequest, ErrorMessages.ItemNameLength);
        }
    }
}