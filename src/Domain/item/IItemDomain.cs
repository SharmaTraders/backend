using Dto;

namespace Domain.item;

public interface IItemDomain {
    Task CreateItem(ItemDto itemDto);
}