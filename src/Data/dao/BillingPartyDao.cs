using Data.converters;
using Data.Entities;
using Domain.dao;
using Dto;
using Microsoft.EntityFrameworkCore;

namespace Data.dao;

public class BillingPartyDao : IBillingPartyDao {
    private readonly DatabaseContext _databaseContext;

    public BillingPartyDao(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }

    public async Task<bool> IsUniqueName(string partyName) {
        return !await _databaseContext.BillingParties
            .AnyAsync(party => party.Name.ToLower().Equals(partyName.ToLower()));
    }

    public async Task CreateBillingParty(CreateBillingPartyRequest request) {
        BillingPartyEntity billingPartyEntity = BillingPartyConverter.ToEntity(request);
        await _databaseContext.BillingParties.AddAsync(billingPartyEntity);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<bool> IsUniqueEmail(string email) {
        return ! await _databaseContext.BillingParties
            .AnyAsync(party => !string.IsNullOrEmpty(party.Email) && party.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<bool> IsUniqueVatNumber(string vatNumber) {
        return  ! await _databaseContext.BillingParties
            .AnyAsync(party =>
                !string.IsNullOrEmpty(party.VatNumber) && party.VatNumber.ToLower().Equals(vatNumber.ToLower()));
    }

    public async Task<ICollection<BillingPartyDto>> GetAllBillingParties() {
        List<BillingPartyEntity> entities = await _databaseContext.BillingParties
            .AsNoTracking()
            .ToListAsync();
        return BillingPartyConverter.ToDtoList(entities);
    }
}