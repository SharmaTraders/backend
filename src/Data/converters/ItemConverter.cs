using Data.Entities;
using Dto;

namespace Data.converters;

public static class ItemConverter
{
    public static ItemDto ToDto(ItemEntity itemEntity)
    {
        return new ItemDto(itemEntity.Name);
    }
    
    public static ICollection<ItemDto> ToDtoList(ICollection<ItemEntity> items)
    {
        return items.Select(ToDto).ToList();
    }
}