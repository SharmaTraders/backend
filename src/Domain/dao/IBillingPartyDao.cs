using Dto;

namespace Domain.dao;

public interface IBillingPartyDao {
    Task<bool> IsUniqueName(string partyName);
    Task CreateBillingParty(CreateBillingPartyRequest request);
    Task<bool> IsUniqueEmail(string email);
    Task<bool> IsUniqueVatNumber(string vatNumber);
    Task<ICollection<BillingPartyDto>> GetAllBillingParties();
}