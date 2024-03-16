using Domain.dao;
using Domain.utils;
using Dto;
using Dto.tools;

namespace Domain.item;

public class ItemDomain : IItemDomain {

    private readonly IItemDao _itemDao;

    public ItemDomain(IItemDao itemDao) {
        _itemDao = itemDao;
    }

    public async Task<ItemDto> CreateItem(ItemDto itemDto) {
        itemDto = new ItemDto(itemDto.Name.Trim());
        ValidateItem(itemDto);

        ItemDto? itemFromDb = await _itemDao.GetItemByName(itemDto.Name);
        if (itemFromDb is not null) {
            throw new ExceptionWithErrorCode(ErrorCode.Conflict, ErrorMessages.ItemNameAlreadyExists); 
        }
        return await _itemDao.CreateItem(itemDto);
    }

    private static void ValidateItem(ItemDto itemDto) {
        if (string.IsNullOrEmpty(itemDto.Name)) {
            throw new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.ItemNameIsRequired);
        }

        if (itemDto.Name.Length is < 3 or > 20) {
            throw new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.ItemNameLength);
        }
    }
}