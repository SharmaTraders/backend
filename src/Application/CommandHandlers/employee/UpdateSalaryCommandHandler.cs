using CommandContracts.employee;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.employee;

public class UpdateSalaryCommandHandler : IRequestHandler<UpdateSalaryCommand.Request> {

    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSalaryCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork) {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task Handle(UpdateSalaryCommand.Request request, CancellationToken cancellationToken) {
        Guid employeeId = GuidParser.ParseGuid(request.Id, "EmployeeId");
        DateOnly startDate = DateParser.ParseDate(request.StartDate);

        EmployeeEntity employee = await _employeeRepository.GetByIdAsync(employeeId)
            ?? throw new DomainValidationException("EmployeeId", ErrorCode.NotFound, ErrorMessages.EmployeeNotFound(employeeId));

        EmployeeSalary salary = new EmployeeSalary() {
            FromDate = startDate,
            OvertimeSalaryPerHour = request.OvertimeSalaryPerHour,
            SalaryPerHour = request.SalaryPerHour,
            ToDate = null
        };

        employee.UpdateSalary(salary);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

    }
}