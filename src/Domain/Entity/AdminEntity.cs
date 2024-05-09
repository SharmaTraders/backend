using System.Text.RegularExpressions;

namespace Domain.Entity;

public class AdminEntity : IEntity<Guid> {

    private string _email;
    private string _password;
     public Guid Id { get; init; }
    
 
    public required string Email {
        get => _email;
        init {
            ValidateEmail(value);
            _email = value;
        }
    }

    public required string Password {
        get => _password;
        init {
            ValidatePassword(value);
            _password = HashPassword(value);
        }
    }

    public bool DoesPasswordMatch(string password) {
        return BCrypt.Net.BCrypt.Verify(password, Password);
    }


    private static void ValidateEmail(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("Email",ErrorCode.BadRequest, ErrorMessages.EmailRequired);
        }

        const string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        Match match = regex.Match(value);

        if (!match.Success) {
            throw new DomainValidationException("Email",ErrorCode.BadRequest, ErrorMessages.EmailInvalidFormat);
        }

    }

    private static void ValidatePassword(string value) {
        if (string.IsNullOrWhiteSpace(value)) {
            throw new DomainValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordRequired);
        }

        if (value.Length < 6) {
            throw new DomainValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordBiggerThan5Characters);
        }

        if (!Regex.IsMatch(value, @"[a-zA-Z]") || !Regex.IsMatch(value, @"[0-9]")) {
            throw new DomainValidationException("Password",ErrorCode.BadRequest, ErrorMessages.PasswordMustContainLetterAndNumber);
        }

    }

    private static string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}