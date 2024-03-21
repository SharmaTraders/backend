using Data.Entities;
using Domain.dao;
using Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.dao;

public class ItemDao : IItemDao {
    private readonly DatabaseContext _databaseContext;

    public ItemDao(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }

    public async Task<ItemDto?> GetItemByName(string itemDtoName) {
        ItemEntity? itemEntity = await _databaseContext.Items!.FirstOrDefaultAsync(entity => entity.Name.ToLower().Equals(itemDtoName.ToLower()));
        return itemEntity is null ? null : new ItemDto(itemEntity.Name);
    }

    public async Task<ItemDto> CreateItem(ItemDto itemDto) {
        ItemEntity itemEntity = new ItemEntity() {
            Name = itemDto.Name
        };
        EntityEntry<ItemEntity> addedItem = await _databaseContext.Items!.AddAsync(itemEntity);
        await _databaseContext.SaveChangesAsync();

        return new ItemDto(addedItem.Entity.Name);
    }
}