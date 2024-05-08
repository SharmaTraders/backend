using System.Runtime.InteropServices;
using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class BillingPartyRepository : IBillingPartyRepository {
    private readonly DatabaseContext _context;

    public BillingPartyRepository(DatabaseContext context) {
        _context = context;
    }

    public async Task AddAsync(BillingPartyEntity entity) {
        await _context.BillingParties.AddAsync(entity);
    }

    public async Task<BillingPartyEntity?> GetByIdAsync(Guid id) {
        return await _context.BillingParties.FindAsync(id);
    }


    public async Task<bool> IsUniqueNameAsync(string name, [Optional] Guid idToExclude) {
        bool doesNameExist = await _context.BillingParties.AnyAsync(bp => bp.Name.ToLower().Equals(name.ToLower())
                                                                          && bp.Id != idToExclude);
        return !doesNameExist;
    }

    public async Task<bool> IsUniqueVatNumberAsync(string vatNumber, [Optional] Guid idToExclude) {
        bool doesVatNumberExist = await _context.BillingParties
            .AnyAsync(party =>
                !string.IsNullOrEmpty(party.VatNumber) && party.VatNumber.ToLower().Equals(vatNumber.ToLower())
                && party.Id != idToExclude);

        return !doesVatNumberExist;
    }

    public Task<List<BillingPartyEntity>> GetAllAsync() {
        return _context.BillingParties.AsNoTracking().ToListAsync();
    }

    public async Task<bool> IsUniqueEmailAsync(string email, [Optional] Guid idToExclude) {
        bool doesEmailExist = await _context.BillingParties
            .AnyAsync(party =>
                !string.IsNullOrEmpty(party.Email) && party.Email.ToLower().Equals(email.ToLower()) &&
                party.Id != idToExclude);
        return !doesEmailExist;
    }
}