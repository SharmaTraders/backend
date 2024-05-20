using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.employee;

namespace Query.QueryHandlers.employee;

public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployees.Query, GetAllEmployees.Answer> {

    private readonly SharmaTradersContext _context;

    public GetAllEmployeesHandler(SharmaTradersContext context) {
        _context = context;
    }

    public async Task<GetAllEmployees.Answer> Handle(GetAllEmployees.Query request, CancellationToken cancellationToken)
    {
        
            var employees = await _context.Employees
            .Select(e => new GetAllEmployees.EmployeeDto(
                e.Id.ToString(),
                e.FullName,
                e.Address,
                e.Email,
                e.PhoneNumber,
                e.Status
            ))
            .ToListAsync(cancellationToken);

        return new GetAllEmployees.Answer(employees);
    }
}