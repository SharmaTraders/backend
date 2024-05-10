using Application.services.billingParty;
using CommandContracts.billingParty;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.billingParty;

public class CreateBillingPartyHandler : IRequestHandler<CreateCommand.Request, CreateCommand.Response> {
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IUniqueBillingPartyEmailChecker _billingPartyEmailChecker;
    private readonly IUniqueBillingPartyVatNumberChecker _billingPartyBillingPartyVatNumberChecker;
    private readonly IUniqueBillingPartyNameChecker _billingPartyNameChecker;


    public CreateBillingPartyHandler(IBillingPartyRepository billingPartyRepository, IUnitOfWork unitOfWork,
        IUniqueBillingPartyNameChecker billingPartyNameChecker, IUniqueBillingPartyVatNumberChecker billingPartyBillingPartyVatNumberChecker,
        IUniqueBillingPartyEmailChecker billingPartyEmailChecker) {
        _billingPartyRepository = billingPartyRepository;
        _unitOfWork = unitOfWork;
        _billingPartyNameChecker = billingPartyNameChecker;
        _billingPartyBillingPartyVatNumberChecker = billingPartyBillingPartyVatNumberChecker;
        _billingPartyEmailChecker = billingPartyEmailChecker;
    }

    public async Task<CreateCommand.Response> Handle(CreateCommand.Request request,
        CancellationToken cancellationToken) {
        BillingPartyEntity partyEntity = new BillingPartyEntity() {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Balance = request.OpeningBalance ?? 0,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            VatNumber = request.VatNumber
        };

        if (!string.IsNullOrEmpty(partyEntity.Name)) {
            bool isUnique = await _billingPartyNameChecker.IsUniqueAsync(partyEntity.Name);
            if (!isUnique) {
                throw new DomainValidationException("Name", ErrorCode.Conflict,
                    ErrorMessages.BillingPartyNameAlreadyExists(partyEntity.Name));
            }
        }

        if (!string.IsNullOrEmpty(partyEntity.Email)) {
            bool isUnique = await _billingPartyEmailChecker.IsUniqueAsync(partyEntity.Email);
            if (!isUnique) {
                throw new DomainValidationException("Email", ErrorCode.Conflict,
                    ErrorMessages.BillingPartyEmailAlreadyExists(partyEntity.Email));
            }
        }

        if (!string.IsNullOrEmpty(partyEntity.VatNumber)) {
            bool isUnique = await _billingPartyBillingPartyVatNumberChecker.IsUniqueAsync(partyEntity.VatNumber);
            if (!isUnique) {
                throw new DomainValidationException("VatNumber", ErrorCode.Conflict,
                    ErrorMessages.BillingPartyVatNumberAlreadyExists(partyEntity.VatNumber));
            }
        }

        await _billingPartyRepository.AddAsync(partyEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new CreateCommand.Response(partyEntity.Id.ToString());
    }
}