using Application.CommandHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.employee;
using Tools;

namespace Query.QueryHandlers.employee;

public class GetEmployeeWorkShiftsHandler : IRequestHandler<GetEmployeeWorkShifts.Query, GetEmployeeWorkShifts.Answer> {
    private readonly SharmaTradersContext _context;

    public GetEmployeeWorkShiftsHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<GetEmployeeWorkShifts.Answer> Handle(GetEmployeeWorkShifts.Query request,
        CancellationToken cancellationToken) {
        Guid employeeId = GuidParser.ParseGuid(request.Id, "EmployeeId");

        int totalCount = await _context.EmployeeWorkShifts
            .Where(w => w.EmployeeEntityId == employeeId)
            .CountAsync(cancellationToken);

        if (totalCount == 0) {
            return new GetEmployeeWorkShifts.Answer(new List<GetEmployeeWorkShifts.EmployeeWorkShiftDto>(1),
                0,
                request.PageNumber,
                request.PageSize
            );
        }

        Employee? employee = await _context.Employees
            .Include(employee => employee.EmployeeWorkShifts
                .OrderByDescending(shift => shift.Date)
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize))
            .Include(employee => employee.EmployeeSalaries)
            .FirstOrDefaultAsync(employee => employee.Id == employeeId, cancellationToken);

        if (employee is null) {
            return new GetEmployeeWorkShifts.Answer(new List<GetEmployeeWorkShifts.EmployeeWorkShiftDto>(1),
                0,
                request.PageNumber,
                request.PageSize
            );
        }

        var workShifts = GetEmployeeWorkShiftDto(employee);
        return new GetEmployeeWorkShifts.Answer(workShifts, totalCount, request.PageNumber, request.PageSize);
    }

    private List<GetEmployeeWorkShifts.EmployeeWorkShiftDto> GetEmployeeWorkShiftDto(Employee employee) {
        List<GetEmployeeWorkShifts.EmployeeWorkShiftDto> workShifts = new();
        foreach (var shift in employee.EmployeeWorkShifts) {
            string name = employee.Name;
            string date = shift.Date.ToString();
            string startTime = shift.StartTime.ToString();
            string endTime = shift.EndTime.ToString();
            int breakMinutes = shift.BreakMinutes;

            double totalWorkedMinutes = shift.EndTime.ToTimeSpan().TotalMinutes
                                        - shift.StartTime.ToTimeSpan().TotalMinutes
                                        - breakMinutes;

            // Subtracting the normal working hours from the total hours worked
            double overtimeHoursWorked = (totalWorkedMinutes
                                          - employee.NormalDailyWorkingMinute) / 60;
            // If its less that 0, then set it to 0
            overtimeHoursWorked = overtimeHoursWorked < 0 ? 0 : overtimeHoursWorked;


            // Find salary for that day
            var salaryForThatDay = employee.EmployeeSalaries.Where(record =>
                                           record.FromDate <= shift.Date &&
                                           (record.ToDate is null || record.ToDate >= shift.Date))
                                       .MaxBy(record => record.FromDate)
                                   ??
                                   throw new DomainValidationException("Salary", ErrorCode.NotFound,
                                       ErrorMessages.SalaryRecordNotFound(shift.Date));

            double normalHoursWorked = totalWorkedMinutes / 60 - overtimeHoursWorked;

            double salaryEarned = normalHoursWorked * salaryForThatDay.SalaryPerHour
                                  + overtimeHoursWorked * salaryForThatDay.OvertimeSalaryPerHour;

            var workShiftDto = new GetEmployeeWorkShifts.EmployeeWorkShiftDto(
                name,
                date,
                startTime,
                endTime,
                breakMinutes,
                Math.Round(overtimeHoursWorked, 2),
                Math.Round(salaryEarned, 2)
            );
            workShifts.Add(workShiftDto);
        }

        return workShifts;
    }
}