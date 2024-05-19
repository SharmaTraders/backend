using Application.services.expense;
using Microsoft.EntityFrameworkCore;

namespace Query.Services.expense;

public class UniqueExpenseNameChecker  : IUniqueExpenseNameChecker{
    private readonly SharmaTradersContext _context;

    public UniqueExpenseNameChecker(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<bool> IsUniqueAsync(string name) {
        bool doesExists = await _context.ExpenseCategories.AnyAsync(category => category.Name.ToLower().Equals(name.ToLower()));
        return !doesExists;
    }
}