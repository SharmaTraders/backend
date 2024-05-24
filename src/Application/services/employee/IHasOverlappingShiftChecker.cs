using Domain.Entity;

namespace Application.services.employee;

public interface IHasOverlappingShiftChecker
{
    public  Task<bool> HasOverlappingShiftAsync(Guid employeeId, EmployeeWorkShift newShift);
}