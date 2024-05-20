using MediatR;

namespace QueryContracts.employee;

public static class GetAllEmployees {
    public record Query() : IRequest<Answer>;

    public record Answer(ICollection<EmployeeDto> Employees);

    public record EmployeeDto(
        string Id,
        string Name,
        string Address,
        string? Email,
        string? PhoneNumber,
        string Status
    );
}