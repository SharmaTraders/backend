using Domain.Entity;

namespace Domain.Repositories;

public interface IBillingPartyRepository : IRepository<BillingPartyEntity, Guid> {
    Task<bool> IsUniqueNameAsync(string name);
    Task<bool> IsUniqueVatNumberAsync(string vatNumber);

    Task<List<BillingPartyEntity>> GetAllAsync();
    Task<bool> IsUniqueEmailAsync(string email);
}