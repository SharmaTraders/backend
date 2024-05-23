using MediatR;

namespace CommandContracts.employee;

public static class AddEmployeeCommand {
    public record Request(
        string Name,
        string Address,
        string? PhoneNumber,
        string? Email,
        double? OpeningBalance,
        string NormalDailyWorkingHours,
        double SalaryPerHour,
        double OvertimeSalaryPerHour
        ) : IRequest<Response>;

    public record Response(string Id);
}