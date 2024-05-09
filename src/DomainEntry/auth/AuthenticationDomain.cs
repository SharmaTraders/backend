using Domain;
using Domain.Entity;
using Domain.Repository;
using DomainEntry.converters;
using Dto;

namespace DomainEntry.auth;

public class AuthenticationDomain : IAuthenticationDomain {

    private readonly IAdminRepository _adminRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AuthenticationDomain(IAdminRepository adminRepository, IUnitOfWork unitOfWork) {
        _adminRepository = adminRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> LoginAdmin(LoginRequest loginRequest) {
        AdminEntity adminEntity = new AdminEntity() {
            Email = loginRequest.Email,
            Password = loginRequest.Password
        };

        AdminEntity? adminFromDb = await _adminRepository.GetByEmailAsync(adminEntity.Email);
        if (adminFromDb is null) {
            throw new DomainValidationException("Email",ErrorCode.NotFound, ErrorMessages.EmailDoesntExist);
        }

        bool doesPasswordMatch = adminFromDb.DoesPasswordMatch(loginRequest.Password);

        if (!doesPasswordMatch) {
            throw new DomainValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordIncorrect);
        }

        return new AdminDto(adminFromDb.Id.ToString(), adminFromDb.Email);
    }

    public async Task RegisterAdmin(RegisterAdminRequest registerAdminRequest) {
        AdminEntity? adminFromDb = await _adminRepository.GetByEmailAsync(registerAdminRequest.Email);
        if (adminFromDb is not null) {
            throw new DomainValidationException("Email",ErrorCode.Conflict, ErrorMessages.EmailAlreadyExists);
        }

        AdminEntity adminEntity = UserConverter.ToEntity(registerAdminRequest);

        await _adminRepository.AddAsync(adminEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    private bool VerifyPassword(string plainPassword, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }

}