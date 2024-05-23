using Application.services.employee;
using CommandContracts.employee;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.employee;

public class RegisterEmployeeWorkShiftHandler : IRequestHandler<RegisterEmployeeWorkShift.Request, RegisterEmployeeWorkShift.Response>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IHasOverlappingShiftChecker _hasOverlappingShiftChecker;
    private readonly IUnitOfWork _unitOfWork;
    
    public RegisterEmployeeWorkShiftHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IHasOverlappingShiftChecker hasOverlappingShiftChecker)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _hasOverlappingShiftChecker = hasOverlappingShiftChecker;
    }
    
    public async Task<RegisterEmployeeWorkShift.Response> Handle(RegisterEmployeeWorkShift.Request request, CancellationToken cancellationToken)
    {
        Guid guid = GuidParser.ParseGuid(request.Id, "Id");
        EmployeeEntity? employeeEntity = await _employeeRepository.GetByIdAsync(guid);
        if (employeeEntity is null)
        {
            throw new DomainValidationException("Id", ErrorCode.NotFound, ErrorMessages.EmployeeNotFound(guid));
        }
        
        EmployeeWorkShift employeeWorkShift = new EmployeeWorkShift()
        {
            StartTime = TimeOnlyParser.ParseTime(request.StartTime),
            EndTime = TimeOnlyParser.ParseTime(request.EndTime),
            Date = DateParser.ParseDate(request.Date),
            BreakMinutes = request.BreakInMinutes
        };

        if (await _hasOverlappingShiftChecker.HasOverlappingShiftAsync(employeeEntity.Id, employeeWorkShift))
        {
            throw new DomainValidationException("WorkShift", ErrorCode.Conflict, ErrorMessages.EmployeeShiftOverlaps);
        }
        
        employeeEntity.AddTimeRecord(employeeWorkShift);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new RegisterEmployeeWorkShift.Response(employeeEntity.Id.ToString());
    }
}