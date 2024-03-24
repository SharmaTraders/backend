using System.Text.RegularExpressions;
using Domain.dao;
using Domain.utils;
using Dto;

namespace Domain.auth;

public class AuthenticationDomain : IAuthenticationDomain {

    private readonly IAuthenticationDao _authDao;
    public AuthenticationDomain(IAuthenticationDao authDao) {
        _authDao = authDao;
    }

    public async Task<UserDto> ValidateAdmin(LoginRequest loginRequest) {
        CheckForValidEmail(loginRequest.Email);
        CheckForValidPassword(loginRequest.Password);

        UserDto? userFromDatabase = await _authDao.GetUserByEmail(loginRequest.Email);
        if (userFromDatabase is null) {
            throw new ValidationException("Email",ErrorCode.NotFound, ErrorMessages.EmailDoesntExist);
        }

        bool doesPasswordMatch = VerifyPassword(loginRequest.Password, userFromDatabase.Password);

        if (!doesPasswordMatch) {
            throw new ValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordIncorrect);
        }

        return userFromDatabase;
    }

    public async Task RegisterAdmin(RegisterAdminRequest registerAdminRequest) {
        CheckForValidEmail(registerAdminRequest.Email);
        CheckForValidPassword(registerAdminRequest.Password);

        string hashedPassword = HashPassword(registerAdminRequest.Password);
        registerAdminRequest = registerAdminRequest with {Password = hashedPassword};

        UserDto? userFromDatabase = await _authDao.GetUserByEmail(registerAdminRequest.Email);
        if (userFromDatabase is not null) {
            throw new ValidationException("Email",ErrorCode.Conflict, ErrorMessages.EmailAlreadyExists);
        }

        await _authDao.RegisterAdmin(registerAdminRequest);
    }


    private string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string plainPassword, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }


    private void CheckForValidPassword(string password) {
        if (string.IsNullOrWhiteSpace(password)) {
            throw new ValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordRequired);
        }

        if (password.Length < 6) {
            throw new ValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordBiggerThan5Characters);
        }

        if (!Regex.IsMatch(password, @"[a-zA-Z]") || !Regex.IsMatch(password, @"[0-9]")) {
            throw new ValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordMustContainLetterAndNumber);
        }
    }

    private void CheckForValidEmail(string email) {
        if (string.IsNullOrEmpty(email)) {
            throw new ValidationException("Email",ErrorCode.BadRequest, ErrorMessages.EmailRequired);
        }

        const string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(email);

        if (!match.Success) {
            throw new ValidationException("Email",ErrorCode.BadRequest, ErrorMessages.EmailInvalidFormat);
        }
    }
}