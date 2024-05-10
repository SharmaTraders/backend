using Application.services.billingParty;
using Microsoft.EntityFrameworkCore;

namespace Query.Services.billingParty;
public class UniqueBillingPartyEmailChecker : IUniqueBillingPartyEmailChecker {


    private readonly SharmaTradersContext _context;

    public UniqueBillingPartyEmailChecker(SharmaTradersContext context) {
        _context = context;
    }

    public async Task<bool> IsUniqueAsync(string email, Guid idToExclude = new Guid()) {
        bool doesExist = await _context.BillingParties
            .Where(x => x.Id != idToExclude
                        && !string.IsNullOrEmpty(x.Email)
                        && x.Email.ToLower().Equals(email.ToLower()))
            .AnyAsync();
        return !doesExist;
    }
}