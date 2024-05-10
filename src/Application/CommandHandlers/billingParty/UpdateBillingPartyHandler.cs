using Application.services.billingParty;
using CommandContracts.billingParty;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.billingParty;

public class UpdateBillingPartyHandler: IRequestHandler<UpdateBillingPartyCommand.Request>{
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IUniqueBillingPartyEmailChecker _billingPartyEmailChecker;
    private readonly IUniqueBillingPartyVatNumberChecker _billingPartyBillingPartyVatNumberChecker;
    private readonly IUniqueBillingPartyNameChecker _billingPartyNameChecker;


    public UpdateBillingPartyHandler(IBillingPartyRepository billingPartyRepository, IUnitOfWork unitOfWork, IUniqueBillingPartyEmailChecker billingPartyEmailChecker, IUniqueBillingPartyVatNumberChecker billingPartyBillingPartyVatNumberChecker, IUniqueBillingPartyNameChecker billingPartyNameChecker) {
        _billingPartyRepository = billingPartyRepository;
        _unitOfWork = unitOfWork;
        _billingPartyEmailChecker = billingPartyEmailChecker;
        _billingPartyBillingPartyVatNumberChecker = billingPartyBillingPartyVatNumberChecker;
        _billingPartyNameChecker = billingPartyNameChecker;
    }

    public async Task Handle(UpdateBillingPartyCommand.Request request, CancellationToken cancellationToken) {

        BillingPartyEntity partyEntity = new BillingPartyEntity() {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            VatNumber = request.VatNumber
        };

        bool tryParse = Guid.TryParse(request.Id, out Guid guid);
        if (!tryParse) {
            throw new DomainValidationException("Id", ErrorCode.BadRequest, ErrorMessages.IdInvalid(request.Id));
        }

        BillingPartyEntity? billingPartyEntity = await _billingPartyRepository.GetByIdAsync(guid);
        if (billingPartyEntity is null) {
            throw new DomainValidationException("Id", ErrorCode.NotFound, ErrorMessages.BillingPartyNotFound(guid));
        }

        if (!string.IsNullOrEmpty(partyEntity.Name)) {
            bool isUnique = await _billingPartyNameChecker.IsUniqueAsync(partyEntity.Name, guid);
            if (!isUnique) {
                throw new DomainValidationException("Email", ErrorCode.Conflict,
                    ErrorMessages.BillingPartyNameAlreadyExists(partyEntity.Name));
            }
        }

        if (!string.IsNullOrEmpty(partyEntity.Email)) {
            bool isUnique = await _billingPartyEmailChecker.IsUniqueAsync(partyEntity.Email, guid);
            if (!isUnique) {
                throw new DomainValidationException("Email", ErrorCode.Conflict,
                    ErrorMessages.BillingPartyEmailAlreadyExists(partyEntity.Email));
            }
        }

        if (!string.IsNullOrEmpty(partyEntity.VatNumber)) {
            bool isUnique = await _billingPartyBillingPartyVatNumberChecker.IsUniqueAsync(partyEntity.VatNumber, guid);
            if (!isUnique) {
                throw new DomainValidationException("Email", ErrorCode.Conflict,
                    ErrorMessages.BillingPartyVatNumberAlreadyExists(partyEntity.VatNumber));
            }
        }

        billingPartyEntity.PhoneNumber = request.PhoneNumber;
        billingPartyEntity.Email = request.Email;
        billingPartyEntity.Address = request.Address;
        billingPartyEntity.Name = request.Name;
        billingPartyEntity.VatNumber = request.VatNumber;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}