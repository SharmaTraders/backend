using System.Runtime.InteropServices;
using Domain.Entity;

namespace Domain.Repository;

public interface IBillingPartyRepository : IRepository<BillingPartyEntity, Guid> {
    
}