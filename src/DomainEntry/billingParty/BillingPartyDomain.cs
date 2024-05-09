using System.Runtime.InteropServices;
using Domain;
using Domain.Entity;
using Domain.Repository;
using DomainEntry.converters;
using Dto;

namespace DomainEntry.billingParty;

public class BillingPartyDomain : IBillingPartyDomain {
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BillingPartyDomain(IBillingPartyRepository billingPartyRepository, IUnitOfWork unitOfWork) {
        _billingPartyRepository = billingPartyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateBillingParty(CreateBillingPartyRequest request) {
        BillingPartyEntity partyEntity = BillingPartyConverter.ToEntity(request);
        await CheckForUniqueName(request.Name);
        await CheckForUniqueVatNumber(request.VatNumber!);
        await CheckForUniqueEmail(request.Email!);

        await _billingPartyRepository.AddAsync(partyEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ICollection<BillingPartyDto>> GetAllBillingParties() {
        List<BillingPartyEntity> billingPartyEntities = await _billingPartyRepository.GetAllAsync();
        return BillingPartyConverter.ToDtoList(billingPartyEntities);
    }

    public async Task UpdateBillingParty(string id, UpdateBillingPartyRequest request) {
        bool tryParse = Guid.TryParse(id, out Guid guid);
        if (!tryParse) {
            throw new DomainValidationException("Id", ErrorCode.BadRequest, ErrorMessages.IdInvalid);
        }

        BillingPartyEntity? billingPartyEntity = await _billingPartyRepository.GetByIdAsync(guid);
        if (billingPartyEntity is null) {
            throw new DomainValidationException("Id", ErrorCode.NotFound, ErrorMessages.BillingPartyNotFound(guid));
        }

        // I tried running the following in parallel but it was causing issues with the database
        await CheckForUniqueName(request.Name, guid);
        await CheckForUniqueVatNumber(request.VatNumber!, guid);
        await CheckForUniqueEmail(request.Email!, guid);

        billingPartyEntity.PhoneNumber = request.PhoneNumber;
        billingPartyEntity.Email = request.Email;
        billingPartyEntity.Address = request.Address;
        billingPartyEntity.Name = request.Name;
        billingPartyEntity.VatNumber = request.VatNumber;
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task CheckForUniqueEmail(string requestEmail, [Optional] Guid idToExclude) {
        bool isUniqueEmail = await _billingPartyRepository.IsUniqueEmailAsync(requestEmail, idToExclude);
        if (!isUniqueEmail) {
            throw new DomainValidationException("Email", ErrorCode.Conflict,
                ErrorMessages.BillingPartyEmailAlreadyExists(requestEmail));
        }
    }

    private async Task CheckForUniqueVatNumber(string requestVatNumber, [Optional] Guid idToExclude) {
        if (string.IsNullOrEmpty(requestVatNumber))
        {
            return;
        }
        bool isUniqueVatNumber = await _billingPartyRepository.IsUniqueVatNumberAsync(requestVatNumber, idToExclude);
        if (!isUniqueVatNumber) {
            throw new DomainValidationException("VatNumber", ErrorCode.Conflict,
                ErrorMessages.BillingPartyVatNumberAlreadyExists(requestVatNumber));
        }
    }

    private async Task CheckForUniqueName(string requestName, [Optional] Guid idToExclude) {
        if (string.IsNullOrEmpty(requestName))
        {
            return;
        }
        bool isUniqueName = await _billingPartyRepository.IsUniqueNameAsync(requestName, idToExclude);
        if (!isUniqueName) {
            throw new DomainValidationException("Name", ErrorCode.Conflict,
                ErrorMessages.BillingPartyNameAlreadyExists(requestName));
        }
    }
}