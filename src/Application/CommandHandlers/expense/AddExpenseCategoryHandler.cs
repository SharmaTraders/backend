using Application.services.expense;
using CommandContracts.expense;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.expense;

public class AddExpenseCategoryHandler : IRequestHandler<AddExpenseCategoryCommand.Request> {
    private readonly IExpenseCategoryRepository _expenseCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueExpenseNameChecker _uniqueExpenseNameChecker;

    public AddExpenseCategoryHandler(IExpenseCategoryRepository expenseCategoryRepository, IUnitOfWork unitOfWork, IUniqueExpenseNameChecker uniqueExpenseNameChecker) {
        _expenseCategoryRepository = expenseCategoryRepository;
        _unitOfWork = unitOfWork;
        _uniqueExpenseNameChecker = uniqueExpenseNameChecker;
    }

    public async Task Handle(AddExpenseCategoryCommand.Request request, CancellationToken cancellationToken) {
        bool isUnique = await _uniqueExpenseNameChecker.IsUniqueAsync(request.Name);
        if (!isUnique) {
            throw new DomainValidationException("Name", ErrorCode.Conflict, ErrorMessages.ExpenseCategoryNameAlreadyExists);
        }

        await _expenseCategoryRepository.AddAsync(new ExpenseCategoryEntity() {
            Name = request.Name
        });
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}