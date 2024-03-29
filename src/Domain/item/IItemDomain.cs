﻿using Dto;

namespace Domain.item;

public interface IItemDomain {
    Task<ItemDto> CreateItem(ItemDto itemDto);
}