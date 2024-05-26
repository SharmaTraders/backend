using MediatR;

namespace QueryContracts.employee;

public static class GetEmployeeWorkShifts {


    public record Query(string Id, int PageNumber, int PageSize) : IRequest<Answer>;

    public record Answer(ICollection<EmployeeWorkShiftDto> WorkShifts ,  int TotalCount, int PageNumber, int PageSize);

    public record EmployeeWorkShiftDto(
        string Name,
        string Date,
        string StartTime,
        string EndTime,
        int BreakMinutes,
        double OverTimeHours,
        double SalaryEarned);

}