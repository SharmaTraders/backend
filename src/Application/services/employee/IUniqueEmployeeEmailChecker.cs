using System.Runtime.InteropServices;

namespace Application.services.employee;

public interface IUniqueEmployeeEmailChecker
{
    Task<bool> IsUniqueAsync(string email, [Optional] Guid idToExclude);

}