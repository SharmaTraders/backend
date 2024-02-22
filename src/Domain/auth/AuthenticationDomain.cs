using System.Text.RegularExpressions;
using Domain.dao;
using Domain.utils;
using Dto;
using Dto.tools;

namespace Domain.auth;

public class AuthenticationDomain(IAuthenticationDao authDao) : IAuthenticationDomain {
    public async Task<UserDto> ValidateAdmin(LoginRequestDto loginRequest) {
        CheckForValidEmail(loginRequest.Email);
        CheckForValidPassword(loginRequest.Password);

        UserDto? userFromDatabase = await authDao.GetOrDefaultUserByEmail(loginRequest.Email);
        if (userFromDatabase is null) {
            throw new ExceptionWithErrorCode(ErrorCode.NotFound, ErrorMessages.EmailDoesntExist);
        }

        bool doesPasswordMatch = VerifyPassword(loginRequest.Password, userFromDatabase.Password);

        if (!doesPasswordMatch) {
            throw new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.IncorrectPassword);
        }

        return userFromDatabase;
    }

    public Task RegisterAdmin(RegisterAdminRequestDto registerAdminRequest) {
        CheckForValidEmail(registerAdminRequest.Email);
        CheckForValidPassword(registerAdminRequest.Password);

        string hashedPassword = HashPassword(registerAdminRequest.Password);

        AdminDto adminToRegister = new AdminDto(Guid.NewGuid().ToString(),
            registerAdminRequest.Email,
            hashedPassword);
        return authDao.RegisterAdmin(adminToRegister);
    }


    private string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string plainPassword, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }


    private void CheckForValidPassword(string password) {
        if (string.IsNullOrWhiteSpace(password)) {
            throw new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.PasswordRequired);
        }

        if (password.Length < 6) {
            throw new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.PasswordBiggerThan5Characters);
        }

        if (!Regex.IsMatch(password, @"[a-zA-Z]") || !Regex.IsMatch(password, @"[0-9]")) {
            throw new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.PasswordMustContainLetterAndNumber);
        }
    }

    private void CheckForValidEmail(string email) {
        if (string.IsNullOrEmpty(email)) {
            throw new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.EmailRequired);
        }

        string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(email);

        if (!match.Success) {
            throw new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.InvalidEmailFormat);
        }
    }
}