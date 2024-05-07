using Dto;

namespace Domain.billingParty;

public interface IBillingPartyDomain {
    Task CreateBillingParty(CreateBillingPartyRequest request);
    Task<ICollection<BillingPartyDto>> GetAllBillingParties();
    Task UpdateBillingParty(string id, UpdateBillingPartyRequest request);
}