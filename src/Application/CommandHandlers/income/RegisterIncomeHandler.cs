using CommandContracts.income;
using Domain.common;
using Domain.DomainServices;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.income;

public class RegisterIncomeHandler : IRequestHandler<RegisterIncomeCommand.Request, RegisterIncomeCommand.Answer> {
    private readonly IIncomeRepository _incomeRepository;
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterIncomeHandler(IIncomeRepository incomeRepository, IBillingPartyRepository billingPartyRepository,
        IUnitOfWork unitOfWork) {
        _incomeRepository = incomeRepository;
        _billingPartyRepository = billingPartyRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<RegisterIncomeCommand.Answer> Handle(RegisterIncomeCommand.Request request,
        CancellationToken cancellationToken) {
        var (date, billingPartyEntity) = await CheckForValidDataExistenceAsync(request);

        IncomeEntity incomeEntity = new IncomeEntity() {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            BillingParty = billingPartyEntity,
            Date = date,
            Remarks = request.Remarks
        };
        AddIncomeService.AddIncome(incomeEntity);
        await _incomeRepository.AddAsync(incomeEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new RegisterIncomeCommand.Answer(incomeEntity.Id.ToString());
    }

    private async Task<(DateOnly, BillingPartyEntity)> CheckForValidDataExistenceAsync(
        RegisterIncomeCommand.Request request) {
        DateOnly date = DateParser.ParseDate(request.Date);
        Guid id = GuidParser.ParseGuid(request.BillingPartyId, "BillingPartyId");

        BillingPartyEntity? partyEntity = await _billingPartyRepository.GetByIdAsync(id);
        if (partyEntity is null) {
            throw new DomainValidationException("BillingPartyId", ErrorCode.NotFound,
                ErrorMessages.BillingPartyNotFound(id));
        }

        return (date, partyEntity);
    }
}