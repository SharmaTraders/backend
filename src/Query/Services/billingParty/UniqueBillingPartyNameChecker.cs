using Application.services.billingParty;
using Microsoft.EntityFrameworkCore;

namespace Query.Services.billingParty;

public class UniqueBillingPartyNameChecker : IUniqueBillingPartyNameChecker{


    private readonly SharmaTradersContext _context;

    public UniqueBillingPartyNameChecker(SharmaTradersContext context) {
        _context = context;
    }

    public async Task<bool> IsUniqueAsync(string name, Guid idToExclude = new Guid()) {
       bool doesExist = await _context.BillingParties
            .Where(x =>x.Id != idToExclude && x.Name.ToLower().Equals(name.ToLower()))
            .AnyAsync();
        return !doesExist; 
    }
}