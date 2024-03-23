using Dto;

namespace Domain.dao;

public interface IBillingPartyDao {
    Task<bool> IsUniqueName(string partyName);
    Task CreateBillingParty(CreateBillingPartyRequestDto request);
    Task<bool> IsUniqueEmail(string email);
    Task<bool> IsUniqueVatNumber(string vatNumber);
}