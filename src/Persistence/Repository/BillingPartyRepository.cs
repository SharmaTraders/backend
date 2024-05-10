using System.Runtime.InteropServices;
using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class BillingPartyRepository : IBillingPartyRepository {
    private readonly WriteDatabaseContext _context;

    public BillingPartyRepository(WriteDatabaseContext context) {
        _context = context;
    }

    public async Task AddAsync(BillingPartyEntity entity) {
        await _context.BillingParties.AddAsync(entity);
    }

    public async Task<BillingPartyEntity?> GetByIdAsync(Guid id) {
        return await _context.BillingParties.FindAsync(id);
    }
}