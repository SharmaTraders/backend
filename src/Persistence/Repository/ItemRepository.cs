using System.Runtime.InteropServices;
using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class ItemRepository : IItemRepository {

    private readonly WriteDatabaseContext _context;
    public ItemRepository(WriteDatabaseContext context) {
        _context = context;
    }

    public async Task AddAsync(ItemEntity entity) {
         await _context.Items.AddAsync(entity);
    }

    public async Task<ItemEntity?> GetByIdAsync(Guid id) {
        return await _context.Items.Include(item => item.StockHistory.Take(5))
            .SingleOrDefaultAsync(item => item.Id == id);
    }

}