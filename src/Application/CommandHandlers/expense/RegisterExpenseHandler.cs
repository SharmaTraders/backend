using CommandContracts.expense;
using Domain.common;
using Domain.DomainServices;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.expense;

public class RegisterExpenseHandler : IRequestHandler<RegisterExpenseCommand.Request, RegisterExpenseCommand.Answer> {
    private readonly IExpenseRepository _expenseRepository;
    private readonly IExpenseCategoryRepository _expenseCategoryRepository;
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterExpenseHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork,
        IExpenseCategoryRepository expenseCategoryRepository,
        IEmployeeRepository employeeRepository,
        IBillingPartyRepository billingPartyRepository) {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _expenseCategoryRepository = expenseCategoryRepository;
        _billingPartyRepository = billingPartyRepository;
        _employeeRepository = employeeRepository;
    }


    public async Task<RegisterExpenseCommand.Answer> Handle(RegisterExpenseCommand.Request request,
        CancellationToken cancellationToken) {
        ExpenseEntity expenseEntity;
        DateOnly date = DateParser.ParseDate(request.Date);


        // When billing party is set,
        if (!string.IsNullOrEmpty(request.BillingPartyId)) {
            Guid id = GuidParser.ParseGuid(request.BillingPartyId, "BillingPartyId");


            // Then we set the category to billing party , if not exists add it
            ExpenseCategoryEntity? partyCategory = await _expenseCategoryRepository.GetByIdAsync("Billing Party");
            if (partyCategory is null) {
                await _expenseCategoryRepository.AddAsync(new ExpenseCategoryEntity() {
                    Name = "Billing Party"
                });
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Recursive call, this time category will be found
                return await Handle(request, cancellationToken);
            }

            BillingPartyEntity partyEntity = await _billingPartyRepository.GetByIdAsync(id)
                                             ??
                                             throw new DomainValidationException("BillingPartyId", ErrorCode.NotFound,
                                                 ErrorMessages.BillingPartyNotFound(id));


            expenseEntity = new ExpenseEntity() {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Category = partyCategory,
                BillingParty = partyEntity,
                Date = date,
                Remarks = request.Remarks
            };
        }

        // When employee is set
        else if (!string.IsNullOrEmpty(request.EmployeeId)) {
            Guid id = GuidParser.ParseGuid(request.EmployeeId, "EmployeeId");

            // Then we set the category to Salary , if not exists add it
            ExpenseCategoryEntity? salaryCategory = await _expenseCategoryRepository.GetByIdAsync("Salary");
            if (salaryCategory is null) {
                await _expenseCategoryRepository.AddAsync(new ExpenseCategoryEntity() {
                    Name = "Salary"
                });
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Recursive call, this time salary will be found
                return await Handle(request, cancellationToken);
            }

            EmployeeEntity employeeEntity = await _employeeRepository.GetByIdAsync(id)
                                             ??
                                             throw new DomainValidationException("EmployeeId", ErrorCode.NotFound,
                                                 ErrorMessages.EmployeeNotFound(id));
            expenseEntity = new ExpenseEntity() {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Category = salaryCategory,
                Employee = employeeEntity,
                Date = date,
                Remarks = request.Remarks
            };
        }
        else if (string.IsNullOrEmpty(request.Category)) {
            // Both category and billing party and employee cannot be null

            throw new DomainValidationException("Name", ErrorCode.BadRequest,
                ErrorMessages.ExpenseEitherCategoryOrBillingPartyRequired);
        }


        else {
            ExpenseCategoryEntity categoryEntity = await _expenseCategoryRepository.GetByIdAsync(request.Category)
                                                   ??
                                                   throw new DomainValidationException("Name", ErrorCode.NotFound,
                                                       ErrorMessages.ExpenseCategoryNotFound(request.Category));
            expenseEntity = new ExpenseEntity() {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Category = categoryEntity,
                Date = date,
                Remarks = request.Remarks,
                BillingParty = null
            };
        }

        RegisterExpenseService.RegisterExpense(expenseEntity);
        await _expenseRepository.AddAsync(expenseEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new RegisterExpenseCommand.Answer(expenseEntity.Id.ToString());
    }
}