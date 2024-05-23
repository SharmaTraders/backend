using Application.services.employee;
using Microsoft.EntityFrameworkCore;

namespace Query.Services.employee;

public class HasOverlappingShiftChecker: IHasOverlappingShiftChecker
{
    private readonly SharmaTradersContext _context;

    public HasOverlappingShiftChecker(SharmaTradersContext context) {
        _context = context;
    }
    
    public async Task<bool> HasOverlappingShiftAsync(Guid employeeId, Domain.Entity.EmployeeWorkShift newShift)
    {
        return await _context.EmployeeWorkShifts
            .AnyAsync(shift =>
                shift.EmployeeEntityId == employeeId &&
                shift.Date == newShift.Date &&
                ((newShift.StartTime < shift.EndTime && newShift.EndTime > shift.StartTime) ||
                 (shift.StartTime < newShift.EndTime && shift.EndTime > newShift.StartTime)));
    }
}