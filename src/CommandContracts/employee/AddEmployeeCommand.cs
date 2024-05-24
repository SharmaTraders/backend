using MediatR;

namespace CommandContracts.employee;

public static class AddEmployeeCommand {
    public record Request(
        string Name,
        string Address,
        string? PhoneNumber,
        string? Email,
        double? OpeningBalance,
        int NormalDailyWorkingMinute,
        double SalaryPerHour,
        double OvertimeSalaryPerHour
        ) : IRequest<Response>;

    public record Response(string Id);
}