using System.Runtime.InteropServices;
using Domain.Entity;

namespace Domain.Repository;

public interface IBillingPartyRepository : IRepository<BillingPartyEntity, Guid> {
    Task<bool> IsUniqueNameAsync(string name, [Optional] Guid idToExclude);
    Task<bool> IsUniqueVatNumberAsync(string vatNumber, [Optional] Guid idToExclude);

    Task<List<BillingPartyEntity>> GetAllAsync();
    Task<bool> IsUniqueEmailAsync(string email, [Optional] Guid idToExclude);
}