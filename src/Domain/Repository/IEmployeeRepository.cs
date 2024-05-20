using Domain.Entity;

namespace Domain.Repository;

public interface IEmployeeRepository : IRepository<EmployeeEntity, Guid>
{
    
}