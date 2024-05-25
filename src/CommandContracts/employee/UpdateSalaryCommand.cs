using MediatR;

namespace CommandContracts.employee;

public static class UpdateSalaryCommand {

    public record Request(
        string Id,
        string StartDate,
        double SalaryPerHour,
        double OvertimeSalaryPerHour
    ) : IRequest;
    
}