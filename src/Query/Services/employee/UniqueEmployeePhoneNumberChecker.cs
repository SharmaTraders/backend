using Application.services.employee;
using Microsoft.EntityFrameworkCore;

namespace Query.Services.employee;

public class UniqueEmployeePhoneNumberChecker : IUniqueEmployeePhoneNumberChecker {

    private readonly SharmaTradersContext _context;

    public UniqueEmployeePhoneNumberChecker(SharmaTradersContext context) {
        _context = context;
    }

    public async Task<bool> IsUniqueAsync(string phoneNumber, Guid idToExclude = new Guid()) {
        bool doesExist = await _context.Employees
            .Where(x => x.Id != idToExclude
                        && !string.IsNullOrEmpty(x.PhoneNumber)
                        && x.PhoneNumber.Equals(phoneNumber))
            .AnyAsync();
        return !doesExist;
    }
}