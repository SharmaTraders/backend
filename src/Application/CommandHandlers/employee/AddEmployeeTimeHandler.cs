using CommandContracts.employee;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.employee;

public class AddEmployeeTimeHandler: IRequestHandler<AddEmployeeTimeCommand.Request>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public AddEmployeeTimeHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(AddEmployeeTimeCommand.Request request, CancellationToken cancellationToken)
    {
        Guid guid = GuidParser.ParseGuid(request.Id, "Id");
        EmployeeEntity? employeeEntity = await _employeeRepository.GetByIdAsync(guid);
        if (employeeEntity is null)
        {
            throw new DomainValidationException("Id", ErrorCode.NotFound, ErrorMessages.EmployeeNotFound(guid));
        }
        
        EmployeeTimeRecord employeeTimeEntity = new EmployeeTimeRecord()
        {
            Id = Guid.NewGuid(),
            StartTime = TimeSpan.Parse(request.StartTime),
            EndTime = TimeSpan.Parse(request.EndTime),
            Date = DateParser.ParseDate(request.Date),
            Break = TimeSpan.Parse(request.BreakTime)
        };
        
        employeeEntity.TimeRecords.Add(employeeTimeEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}