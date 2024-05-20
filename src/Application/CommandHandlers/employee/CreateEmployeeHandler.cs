﻿using Application.services.employee;
using CommandContracts.employee;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.employee;

public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand.Request, CreateEmployeeCommand.Response> {
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IUniqueEmployeeEmailChecker _employeeEmailChecker;
    private readonly IUniqueEmployeePhoneNumberChecker _employeePhoneNumberChecker;

    public CreateEmployeeHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork,
        IUniqueEmployeeEmailChecker employeeEmailChecker, IUniqueEmployeePhoneNumberChecker employeePhoneNumberChecker) {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _employeeEmailChecker = employeeEmailChecker;
        _employeePhoneNumberChecker = employeePhoneNumberChecker;
    }

    public async Task<CreateEmployeeCommand.Response> Handle(CreateEmployeeCommand.Request request,
        CancellationToken cancellationToken) {
        EmployeeEntity employeeEntity = new EmployeeEntity() {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Status = request.Status
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

        await _employeeRepository.AddAsync(employeeEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new CreateEmployeeCommand.Response(employeeEntity.Id.ToString());
    }
}
