using System.Runtime.InteropServices;

namespace Application.services.employee;

public interface IUniqueEmployeePhoneNumberChecker
{
    Task<bool> IsUniqueAsync(string phoneNumber, [Optional] Guid idToExclude);
}