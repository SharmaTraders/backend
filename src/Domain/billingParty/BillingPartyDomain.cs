﻿using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Domain.converters;
using Domain.Entity;
using Domain.Repository;
using Dto;
using Domain.utils;

namespace Domain.billingParty;

public class BillingPartyDomain : IBillingPartyDomain {
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BillingPartyDomain(IBillingPartyRepository billingPartyRepository, IUnitOfWork unitOfWork) {
        _billingPartyRepository = billingPartyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateBillingParty(CreateBillingPartyRequest request) {
        BillingPartyEntity partyEntity = BillingPartyConverter.ToEntity(request);
        List<Task> tasks = [
            CheckForUniqueName(request.Name),
            CheckForUniqueVatNumber(request.VatNumber!),
            CheckForUniqueEmail(request.Email!)
        ];

        // Wait for all tasks to complete
        await Task.WhenAll(tasks);       

        await _billingPartyRepository.AddAsync(partyEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ICollection<BillingPartyDto>> GetAllBillingParties() {
        List<BillingPartyEntity>  billingPartyEntities = await _billingPartyRepository.GetAllAsync();
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

        List<Task> tasks = [
            CheckForUniqueName(request.Name, guid),
            CheckForUniqueVatNumber(request.VatNumber!, guid),
            CheckForUniqueEmail(request.Email!, guid)
        ];

        // Wait for all tasks to complete
        await Task.WhenAll(tasks);

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
            throw new DomainValidationException("Email", ErrorCode.Conflict, ErrorMessages.BillingPartyEmailAlreadyExists(requestEmail));
        }
        
    }

    private async Task CheckForUniqueVatNumber(string requestVatNumber, [Optional] Guid idToExclude) {
        bool isUniqueVatNumber = await _billingPartyRepository.IsUniqueVatNumberAsync(requestVatNumber, idToExclude);
        if (!isUniqueVatNumber) {
            throw new DomainValidationException("VatNumber", ErrorCode.Conflict, ErrorMessages.BillingPartyVatNumberAlreadyExists(requestVatNumber));
        }
    }

    private async Task CheckForUniqueName(string requestName, [Optional] Guid idToExclude) {
        bool isUniqueName = await _billingPartyRepository.IsUniqueNameAsync(requestName, idToExclude);
        if (!isUniqueName) {
            throw new DomainValidationException("Name", ErrorCode.Conflict, ErrorMessages.BillingPartyNameAlreadyExists(requestName));
        }
    }

 

   




  

    
}