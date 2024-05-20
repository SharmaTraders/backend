using Application.services.employee;
using Microsoft.EntityFrameworkCore;

namespace Query.Services.employee;

public class UniqueEmployeeEmailChecker : IUniqueEmployeeEmailChecker {

    private readonly SharmaTradersContext _context;

    public UniqueEmployeeEmailChecker(SharmaTradersContext context) {
        _context = context;
    }

    public async Task<bool> IsUniqueAsync(string email, Guid idToExclude = new Guid()) {
        bool doesExist = await _context.Employees
            .Where(x => x.Id != idToExclude
                        && !string.IsNullOrEmpty(x.Email)
                        && x.Email.ToLower().Equals(email.ToLower()))
            .AnyAsync();
        return !doesExist;
    }
}