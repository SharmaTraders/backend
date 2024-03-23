using Domain.dao;
using Domain.utils;
using Dto;

namespace Domain.item;

public class ItemDomain : IItemDomain {

    private readonly IItemDao _itemDao;

    public ItemDomain(IItemDao itemDao) {
        _itemDao = itemDao;
    }

    public async Task CreateItem(ItemDto itemDto) {
        itemDto = new ItemDto(itemDto.Name.Trim());
        ValidateItem(itemDto);

        ItemDto? itemFromDb = await _itemDao.GetItemByName(itemDto.Name);
        if (itemFromDb is not null) {
            throw new ValidationException("ItemName",ErrorCode.Conflict, ErrorMessages.ItemNameAlreadyExists(itemDto.Name)); 
        } 
        await _itemDao.CreateItem(itemDto);
    }

    private static void ValidateItem(ItemDto itemDto) {
        if (string.IsNullOrEmpty(itemDto.Name)) {
            throw new ValidationException("ItemName",ErrorCode.BadRequest, ErrorMessages.ItemNameIsRequired);
        }

        if (itemDto.Name.Length is < 3 or > 20) {
            throw new ValidationException("ItemName",ErrorCode.BadRequest, ErrorMessages.ItemNameLength);
        }
    }
}