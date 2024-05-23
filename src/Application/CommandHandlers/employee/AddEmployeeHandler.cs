﻿using Application.services.employee;
using CommandContracts.employee;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.employee;

public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand.Request, AddEmployeeCommand.Response> {
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueEmployeeEmailChecker _employeeEmailChecker;
    private readonly IUniqueEmployeePhoneNumberChecker _employeePhoneNumberChecker;

    public AddEmployeeHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork,
        IUniqueEmployeeEmailChecker employeeEmailChecker, IUniqueEmployeePhoneNumberChecker employeePhoneNumberChecker) {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _employeeEmailChecker = employeeEmailChecker;
        _employeePhoneNumberChecker = employeePhoneNumberChecker;
    }

    public async Task<AddEmployeeCommand.Response> Handle(AddEmployeeCommand.Request request,
        CancellationToken cancellationToken) {
        EmployeeEntity employeeEntity = new EmployeeEntity() {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Status = EmployeeStatusCategory.Active,
            Balance = request.OpeningBalance ?? 0,
            NormalDailyWorkingMinute = request.NormalDailyWorkingMinute,
        };
        
        EmployeeSalaryRecord salaryRecord = new EmployeeSalaryRecord() {
            FromDate = DateOnly.MinValue,
            SalaryPerHr = request.SalaryPerHour,
            OvertimeSalaryPerHr = request.OvertimeSalaryPerHour
        };
        
        if (!string.IsNullOrEmpty(employeeEntity.Email)) {
            bool isUnique = await _employeeEmailChecker.IsUniqueAsync(employeeEntity.Email);
            if (!isUnique) {
                throw new DomainValidationException("Email", ErrorCode.Conflict,
                    ErrorMessages.EmployeeEmailAlreadyExists(employeeEntity.Email));
            }
        }

        if (!string.IsNullOrEmpty(employeeEntity.PhoneNumber)) {
            bool isUnique = await _employeePhoneNumberChecker.IsUniqueAsync(employeeEntity.PhoneNumber);
            if (!isUnique) {
                throw new DomainValidationException("PhoneNumber", ErrorCode.Conflict,
                    ErrorMessages.EmployeePhoneNumberAlreadyExists(employeeEntity.PhoneNumber));
            }
        }
        
        employeeEntity.AddSalaryRecord(salaryRecord);
        
        await _employeeRepository.AddAsync(employeeEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AddEmployeeCommand.Response(employeeEntity.Id.ToString());
    }
}
