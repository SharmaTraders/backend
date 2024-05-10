using System.Runtime.InteropServices;

namespace Application.services.billingParty;

public interface IUniqueBillingPartyVatNumberChecker {
    Task<bool> IsUniqueAsync(string vatNumber, [Optional] Guid idToExclude);
    
}