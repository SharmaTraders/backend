using Dto;

namespace Domain.billingParty;

public interface IBillingPartyDomain {
    Task CreateBillingParty(CreateBillingPartyRequest request);
    Task<ICollection<BillingPartyDto>> GetBillingParties();
}