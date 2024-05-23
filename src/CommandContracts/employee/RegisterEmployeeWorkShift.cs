using MediatR;

namespace CommandContracts.employee;

public static class RegisterEmployeeWorkShift
{
    public record Request(
        string Id,
        string StartTime,
        string EndTime,
        string Date,
        int BreakInMinutes) : IRequest<Response>;
    
    public record Response(string Id);

}