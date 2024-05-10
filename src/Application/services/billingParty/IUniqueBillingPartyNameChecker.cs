using System.Runtime.InteropServices;

namespace Application.services.billingParty;

public interface IUniqueBillingPartyNameChecker {
    Task<bool> IsUniqueAsync(string name, [Optional] Guid idToExclude);
    
}