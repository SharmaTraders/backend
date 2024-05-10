using Application.services.billingParty;
using Microsoft.EntityFrameworkCore;

namespace Query.Services.billingParty;

public class UniqueBillingPartyVatNumberChecker : IUniqueBillingPartyVatNumberChecker {

    private readonly SharmaTradersContext _context;

    public UniqueBillingPartyVatNumberChecker(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<bool> IsUniqueAsync(string vatNumber, Guid idToExclude = new Guid()) {
        bool doesExist = await _context.BillingParties
            .Where(x => x.Id != idToExclude
                        && !string.IsNullOrEmpty(x.VatNumber)
                        && x.VatNumber.ToLower().Equals(vatNumber.ToLower()))
            .AnyAsync();
        return !doesExist;
    }
}