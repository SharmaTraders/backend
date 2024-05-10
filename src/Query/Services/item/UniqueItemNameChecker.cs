using Application.services.item;
using Microsoft.EntityFrameworkCore;

namespace Query.Services.item;

public class UniqueItemNameChecker : IUniqueItemNameChecker{

    private readonly SharmaTradersContext _context;

    public UniqueItemNameChecker(SharmaTradersContext context) {
        _context = context;
    }

    public async Task<bool> IsUniqueAsync(string name, Guid idToExclude = new Guid()) {
        bool doesExist = await _context.Items
            .Where(x =>x.Id != idToExclude && x.Name.ToLower().Equals(name.ToLower()))
            .AnyAsync();

        return !doesExist;
    }
}