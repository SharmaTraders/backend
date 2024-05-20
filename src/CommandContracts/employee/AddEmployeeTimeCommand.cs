using MediatR;

namespace CommandContracts.employee;

public static class AddEmployeeTimeCommand
{
    public record Request(
        string Id,
        string StartTime,
        string EndTime,
        string Date,
        string BreakTime) : IRequest;
}