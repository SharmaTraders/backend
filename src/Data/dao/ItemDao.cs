using Data.Entities;
using Domain.dao;
using Dto;
using Microsoft.EntityFrameworkCore;

namespace Data.dao;

public class ItemDao : IItemDao {
    private readonly DatabaseContext _databaseContext;

    public ItemDao(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }

    public async Task<ItemDto?> GetItemByName(string itemDtoName) {
        ItemEntity? itemEntity =
            await _databaseContext.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(entity =>
                entity.Name.ToLower().Equals(itemDtoName.ToLower()));
        return itemEntity is null ? null : new ItemDto(itemEntity.Name);
    }

    public async Task CreateItem(ItemDto itemDto) {
        ItemEntity itemEntity = new ItemEntity() {
            Name = itemDto.Name
        };
        await _databaseContext.Items.AddAsync(itemEntity);
        await _databaseContext.SaveChangesAsync();
    }
}