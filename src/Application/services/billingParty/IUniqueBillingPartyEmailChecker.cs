using System.Runtime.InteropServices;

namespace Application.services.billingParty;

public interface IUniqueBillingPartyEmailChecker {
    Task<bool> IsUniqueAsync(string email, [Optional] Guid idToExclude);
    
}