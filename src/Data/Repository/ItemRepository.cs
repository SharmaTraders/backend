using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class ItemRepository : IItemRepository {

    private readonly DatabaseContext _context;

    public ItemRepository(DatabaseContext context) {
        _context = context;
    }

    public async Task AddAsync(ItemEntity entity) {
         await _context.Items.AddAsync(entity);
    }

    public async Task<ItemEntity?> GetByIdAsync(Guid id) {
        return await _context.Items.FindAsync(id);
    }

    public async Task<ItemEntity?> GetByNameAsync(string name) {
        return await _context.Items.FirstOrDefaultAsync(item => item.Name.ToLower().Equals(name.ToLower()));
    }

    public Task<List<ItemEntity>> GetAllAsync() {
        return _context.Items.AsNoTracking().ToListAsync();
    }
}