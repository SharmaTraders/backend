﻿using System.Globalization;
using System.Text.RegularExpressions;
using Domain.dao;
using Dto;
using Domain.utils;

namespace Domain.billingParty;

public class BillingPartyDomain : IBillingPartyDomain {
    private readonly IBillingPartyDao _billingPartyDao;

    public BillingPartyDomain(IBillingPartyDao billingPartyDao) {
        _billingPartyDao = billingPartyDao;
    }

    public async Task CreateBillingParty(CreateBillingPartyRequestDto request) {
        await ValidateName(request.Name);
        await ValidateEmail(request.Email);
        await ValidateVatNumber(request.VatNumber);
        ValidateAddress(request.Address);
        ValidatePhoneNumber(request.PhoneNumber);
        ValidateOpeningBalance(request.OpeningBalance);

        await _billingPartyDao.CreateBillingParty(request);
    }

    private async Task ValidateName(string name) {
        if (string.IsNullOrEmpty(name)) {
            throw new ValidationException("Name", ErrorCode.BadRequest, ErrorMessages.BillingPartyNameIsRequired);
        }

        //Length between 3 and 30 characters (inclusive)
        if (name.Length is < 3 or > 30) {
            throw new ValidationException("Name", ErrorCode.BadRequest, ErrorMessages.BillingPartyNameBetween3And30);
        }

        bool uniqueName = await _billingPartyDao.IsUniqueName(name);
        if (!uniqueName) {
            throw new ValidationException("Name", ErrorCode.Conflict,
                ErrorMessages.BillingPartyNameAlreadyExists(name));
        }
    }

    private void ValidateAddress(string address) {
        if (string.IsNullOrEmpty(address)) {
            throw new ValidationException("Address", ErrorCode.BadRequest, ErrorMessages.BillingPartyAddressIsRequired);
        }

        //Length between 3 and 30 characters (inclusive)
        if (address.Length is < 3 or > 60) {
            throw new ValidationException("Address", ErrorCode.BadRequest, ErrorMessages.BillingPartyNameIsRequired);
        }
    }

    private void ValidatePhoneNumber(string? phoneNumber) {
        // Phone number is optional
        if (string.IsNullOrEmpty(phoneNumber)) {
            return;
        }

        if (!Regex.IsMatch(phoneNumber, @"^\d+$")) {
            throw new ValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.PhoneNumberMustBeAllDigits);
        }

        if (phoneNumber.Length != 10) {
            throw new ValidationException("PhoneNumber", ErrorCode.BadRequest,
                ErrorMessages.PhoneNumberMustBe10DigitsLong);
        }
    }

    private async Task ValidateEmail(string? email) {
        // Email  is optional
        if (string.IsNullOrEmpty(email)) {
            return;
        }

        const string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(email);

        if (!match.Success) {
            throw new ValidationException("Email", ErrorCode.BadRequest, ErrorMessages.EmailInvalidFormat);
        }

        bool uniqueEmail = await _billingPartyDao.IsUniqueEmail(email);
        if (!uniqueEmail) {
            throw new ValidationException("Email", ErrorCode.Conflict,
                ErrorMessages.BillingPartyEmailAlreadyExists(email));
        }
    }

    private void ValidateOpeningBalance(double openingBalance) {
        if (openingBalance < 0) {
            throw new ValidationException("OpeningBalance", ErrorCode.BadRequest,
                ErrorMessages.OpeningBalanceMustBePositive);
        }

        // Makes sure that upto two digits after decimal is acceptable, -- generated by ChatGPT , dont ask me how 🤣🤣
        string balanceStr = openingBalance.ToString(CultureInfo.InvariantCulture);
        int decimalSeparatorIndex = balanceStr.IndexOf('.');
        if (decimalSeparatorIndex < 0) {
            return;
        }

        if (!(balanceStr.Length - decimalSeparatorIndex - 1 <= 2)) {
            throw new ValidationException("OpeningBalance", ErrorCode.BadRequest,
                ErrorMessages.OpeningBalanceMustBeAtMax2DecimalPlaces);
        }
    }

    private async Task ValidateVatNumber(string? vatNumber) {
        // Vat number  is optional
        if (string.IsNullOrEmpty(vatNumber)) {
            return;
        }

        if (vatNumber.Length is < 5 or > 20) {
            throw new ValidationException("VatNumber", ErrorCode.BadRequest,
                ErrorMessages.VatNumberMustBeBetween5To20Characters);
        }


        bool isUniqueVatNumber = await _billingPartyDao.IsUniqueVatNumber(vatNumber);
        if (!isUniqueVatNumber) {
            throw new ValidationException("Email", ErrorCode.Conflict,
                ErrorMessages.BillingPartyVatNumberAlreadyExists(vatNumber));
        }
    }
}