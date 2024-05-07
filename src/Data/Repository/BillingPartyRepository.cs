using Domain.Entity;
using Domain.Repositories;
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
        return await _context.BillingParties.FindAsync(id.ToString());
    }

    public async Task<bool> IsUniqueNameAsync(string name) {
        bool doesNameExist = await _context.BillingParties.AnyAsync(bp => bp.Name.ToLower().Equals(name.ToLower()));
        return !doesNameExist;
    }

    public async Task<bool> IsUniqueVatNumberAsync(string vatNumber) {
        bool doesVatNumberExist = await _context.BillingParties
            .AnyAsync(party =>
                !string.IsNullOrEmpty(party.VatNumber) && party.VatNumber.ToLower().Equals(vatNumber.ToLower()));

        return !doesVatNumberExist;
    }


    public Task<List<BillingPartyEntity>> GetAllAsync() {
        return _context.BillingParties.AsNoTracking().ToListAsync();
    }

    public async Task<bool> IsUniqueEmailAsync(string email) {
        bool doesEmailExist = await _context.BillingParties
            .AnyAsync(party =>
                !string.IsNullOrEmpty(party.Email) && party.Email.ToLower().Equals(email.ToLower()));

        return !doesEmailExist;
    }
}
