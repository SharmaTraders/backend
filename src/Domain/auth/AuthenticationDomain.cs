using System.Text.RegularExpressions;
using Domain.converters;
using Domain.Entity;
using Domain.Repositories;
using Domain.utils;
using Dto;

namespace Domain.auth;

public class AuthenticationDomain : IAuthenticationDomain {

    private readonly IAdminRepository _adminRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AuthenticationDomain(IAdminRepository adminRepository, IUnitOfWork unitOfWork) {
        _adminRepository = adminRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> ValidateAdmin(LoginRequest loginRequest) {
        CheckForValidEmail(loginRequest.Email);
        CheckForValidPassword(loginRequest.Password);

        AdminEntity? adminFromDb = await _adminRepository.GetByEmailAsync(loginRequest.Email);
        if (adminFromDb is null) {
            throw new DomainValidationException("Email",ErrorCode.NotFound, ErrorMessages.EmailDoesntExist);
        }

        bool doesPasswordMatch = VerifyPassword(loginRequest.Password, adminFromDb.Password);

        if (!doesPasswordMatch) {
            throw new DomainValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordIncorrect);
        }

        return new AdminDto(adminFromDb.Id.ToString(), adminFromDb.Email);
    }

    public async Task RegisterAdmin(RegisterAdminRequest registerAdminRequest) {
        CheckForValidEmail(registerAdminRequest.Email);
        CheckForValidPassword(registerAdminRequest.Password);

        string hashedPassword = HashPassword(registerAdminRequest.Password);
        registerAdminRequest = registerAdminRequest with {Password = hashedPassword};

        AdminEntity? adminFromDb = await _adminRepository.GetByEmailAsync(registerAdminRequest.Email);
        if (adminFromDb is not null) {
            throw new DomainValidationException("Email",ErrorCode.Conflict, ErrorMessages.EmailAlreadyExists);
        }

        AdminEntity adminEntity = UserConverter.ToEntity(registerAdminRequest);

        await _adminRepository.AddAsync(adminEntity);
        await _unitOfWork.SaveChangesAsync();
    }


    private string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string plainPassword, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }


    private void CheckForValidPassword(string password) {
        if (string.IsNullOrWhiteSpace(password)) {
            throw new DomainValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordRequired);
        }

        if (password.Length < 6) {
            throw new DomainValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordBiggerThan5Characters);
        }

        if (!Regex.IsMatch(password, @"[a-zA-Z]") || !Regex.IsMatch(password, @"[0-9]")) {
            throw new DomainValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordMustContainLetterAndNumber);
        }
    }

    private void CheckForValidEmail(string email) {
        if (string.IsNullOrEmpty(email)) {
            throw new DomainValidationException("Email",ErrorCode.BadRequest, ErrorMessages.EmailRequired);
        }

        const string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(email);

        if (!match.Success) {
            throw new DomainValidationException("Email",ErrorCode.BadRequest, ErrorMessages.EmailInvalidFormat);
        }
    }
}