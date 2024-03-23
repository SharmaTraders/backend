using Dto;

namespace Domain.billingParty;

public interface IBillingPartyDomain {
    Task CreateBillingParty(CreateBillingPartyRequestDto request);
}