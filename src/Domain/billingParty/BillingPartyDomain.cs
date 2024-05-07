using System.Text.RegularExpressions;
using Domain.converters;
using Domain.Entity;
using Domain.Repositories;
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
        await ValidateBillingParty(
            request.Name, request.Email, request.VatNumber, request.Address, request.PhoneNumber,
            request.OpeningBalance);

        BillingPartyEntity partyEntity = BillingPartyConverter.ToEntity(request);
        await _billingPartyRepository.AddAsync(partyEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ICollection<BillingPartyDto>> GetAllBillingParties() {
        List<BillingPartyEntity>  billingPartyEntities = await _billingPartyRepository.GetAllAsync();
        return BillingPartyConverter.ToDtoList(billingPartyEntities);
    }

    public async Task UpdateBillingParty(string id, UpdateBillingPartyRequest request) {
        await ValidateBillingParty(
            request.Name, request.Email, request.VatNumber, request.Address, request.PhoneNumber,
            null);

        bool tryParse = Guid.TryParse(id, out Guid guid);
        if (!tryParse) {
            throw new DomainValidationException("Id", ErrorCode.BadRequest, ErrorMessages.IdInvalid);
        }

        BillingPartyEntity? billingPartyEntity = await _billingPartyRepository.GetByIdAsync(guid);
        if (billingPartyEntity is null) {
            throw new DomainValidationException("Id", ErrorCode.NotFound, ErrorMessages.BillingPartyNotFound(guid));
        }

        billingPartyEntity.PhoneNumber = request.PhoneNumber;
        billingPartyEntity.Email = request.Email;
        billingPartyEntity.Address = request.Address;
        billingPartyEntity.Name = request.Name;
        billingPartyEntity.VatNumber = request.VatNumber;
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task ValidateBillingParty(
        string name, string? email, string? vatNumber, string? address, string? phoneNumber, double? openingBalance) {
        await ValidateName(name);
        await ValidateEmail(email);
        await ValidateVatNumber(vatNumber);
        ValidateAddress(address);
        ValidatePhoneNumber(phoneNumber);
        ValidateOpeningBalance(openingBalance);
    }


    /*
     * The validate methods are made internal so that they can be tested in unit tests
     * I think treating them as individual tests made more sense to me ,otherwise there would be a lot of
     *     duplication in the tests, and we would end up with 6! (six factorial) tests for the CreateBillingParty method
     *         in order to fulfill all the branches which is infeasible.
     *
     * But this need to be noted, that these are private otherwise and must not be called even from the assembly.
     */

    internal async Task ValidateName(string name) {
        if (string.IsNullOrEmpty(name)) {
            throw new DomainValidationException("Name", ErrorCode.BadRequest, ErrorMessages.BillingPartyNameIsRequired);
        }

        //Length between 3 and 30 characters (inclusive)
        if (name.Length is < 3 or > 30) {
            throw new DomainValidationException("Name", ErrorCode.BadRequest, ErrorMessages.BillingPartyNameBetween3And30);
        }

        bool uniqueName = await _billingPartyRepository.IsUniqueNameAsync(name);
        if (!uniqueName) {
            throw new DomainValidationException("Name", ErrorCode.Conflict,
                ErrorMessages.BillingPartyNameAlreadyExists(name));
        }
    }

    internal void ValidateAddress(string? address) {
        if (string.IsNullOrEmpty(address)) {
            throw new DomainValidationException("Address", ErrorCode.BadRequest, ErrorMessages.BillingPartyAddressIsRequired);
        }

        //Length between 3 and 30 characters (inclusive)
        if (address.Length is < 3 or > 60) {
            throw new DomainValidationException("Address", ErrorCode.BadRequest, ErrorMessages.BillingPartyAddressBetween3And60);
        }
    }

    internal void ValidatePhoneNumber(string? phoneNumber) {
        // Phone number is optional
        if (string.IsNullOrEmpty(phoneNumber)) {
            return;
        }

        if (!Regex.IsMatch(phoneNumber, @"^\d+$")) {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.PhoneNumberMustBeAllDigits);
        }

        if (phoneNumber.Length != 10) {
            throw new DomainValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.PhoneNumberMustBe10DigitsLong);
        }
    }

    internal async Task ValidateEmail(string? email) {
        // Email  is optional
        if (string.IsNullOrEmpty(email)) {
            return;
        }

        const string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(email);

        if (!match.Success) {
            throw new DomainValidationException("Email", ErrorCode.BadRequest, ErrorMessages.EmailInvalidFormat);
        }

        bool uniqueEmail = await _billingPartyRepository.IsUniqueEmailAsync(email);
        if (!uniqueEmail) {
            throw new DomainValidationException("Email", ErrorCode.Conflict,
                ErrorMessages.BillingPartyEmailAlreadyExists(email));
        }
    }

    internal void ValidateOpeningBalance(double? openingBalance) {
        if (openingBalance is null) return;

        // Makes sure that upto two digits after decimal is acceptable
        string balanceStr = openingBalance.ToString()!;
        int decimalSeparatorIndex = balanceStr.IndexOf('.');
        if (decimalSeparatorIndex < 0) {
            return;
        }

        if (!(balanceStr.Length - decimalSeparatorIndex - 1 <= 2)) {
            throw new DomainValidationException("OpeningBalance", ErrorCode.BadRequest,
                ErrorMessages.OpeningBalanceMustBeAtMax2DecimalPlaces);
        }
    }

    internal async Task ValidateVatNumber(string? vatNumber) {
        // Vat number  is optional
        if (string.IsNullOrEmpty(vatNumber)) {
            return;
        }

        if (vatNumber.Length is < 5 or > 20) {
            throw new DomainValidationException("VatNumber", ErrorCode.BadRequest,
                ErrorMessages.VatNumberMustBeBetween5To20Characters);
        }


        bool isUniqueVatNumber = await _billingPartyRepository.IsUniqueVatNumberAsync(vatNumber);
        if (!isUniqueVatNumber) {
            throw new DomainValidationException("VatNumber", ErrorCode.Conflict,
                ErrorMessages.BillingPartyVatNumberAlreadyExists(vatNumber));
        }
    }
}