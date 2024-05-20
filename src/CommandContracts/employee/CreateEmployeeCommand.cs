using MediatR;

namespace CommandContracts.employee;

public static class CreateEmployeeCommand {
    public record Request(
        string FullName,
        string Address,
        string? PhoneNumber,
        string? Email,
        string Status) : IRequest<Response>;

    public record Response(string Id);
}