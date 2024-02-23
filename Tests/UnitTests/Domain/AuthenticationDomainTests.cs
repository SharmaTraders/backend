using Domain.auth;
using Domain.dao;
using Domain.utils;
using Dto;
using Dto.tools;
using Moq;

namespace UnitTests.Domain;

public class AuthenticationDomainTests {
    [Fact]
    public async Task ValidateAdmin_WithValidCredentials_ReturnsUserDto() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var userDto = new AdminDto("id", "email@email.com", HashPassword("password1"));

        authDaoMock.Setup(mock => mock.GetOrDefaultUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(userDto);

        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto("email@email.com", "password1");

        // Act
        var result = await authenticationDomain.ValidateAdmin(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto, result);
        // Add more assertions based on your expected behavior
    }

    [Fact]
    public async Task ValidateAdmin_WithNonexistentEmail_ThrowsException() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        authDaoMock.Setup(mock => mock.GetOrDefaultUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(null as AdminDto);

        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto("email@email.com", "password1");

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ExceptionWithErrorCode>(() => authenticationDomain.ValidateAdmin(loginRequest));

        Assert.Equal(ErrorCode.NotFound, exception.ErrorCode);
        Assert.Equal(ErrorMessages.EmailDoesntExist, exception.Message);
    }

    [Fact]
    public async Task ValidateAdmin_WithIncorrectPassword_ThrowsException() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        AdminDto adminDto = new AdminDto("id", "email@email.com", HashPassword("password1"));

        authDaoMock.Setup(mock => mock.GetOrDefaultUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(adminDto);

        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto("email@email.com", "incorrectPassword1");

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ExceptionWithErrorCode>(() => authenticationDomain.ValidateAdmin(loginRequest));

        // Assert the exception properties
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.IncorrectPassword, exception.Message);
    }


    [Fact]
    public async Task RegisterAdmin_WithValidData_CallsDaoRegisterAdmin() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var registerAdminRequest = new RegisterAdminRequestDto("valid@email.com", "validPassword123");

        // Act
        await authenticationDomain.RegisterAdmin(registerAdminRequest);

        // Assert
        authDaoMock.Verify(mock => mock.RegisterAdmin(It.IsAny<AdminDto>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAdmin_WithInvalidEmail_ThrowsException() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var registerAdminRequest = new RegisterAdminRequestDto("invalid", "validPassword123");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExceptionWithErrorCode>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(exception.ErrorCode, ErrorCode.BadRequest);
        Assert.Equal(exception.Message, ErrorMessages.InvalidEmailFormat);
    }

    [Fact]
    public async Task RegisterAdmin_WithInvalidPassword_Short_ThrowsException() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var registerAdminRequest = new RegisterAdminRequestDto("valid@email.com", "short");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExceptionWithErrorCode>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(exception.ErrorCode, ErrorCode.BadRequest);
        Assert.Equal(exception.Message, ErrorMessages.PasswordBiggerThan5Characters);
    }

    [Fact]
    public async Task RegisterAdmin_WithInvalidPassword_NoLetterAndNumber_ThrowsException() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var registerAdminRequest = new RegisterAdminRequestDto("valid@email.com", "longWithoutNumber");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExceptionWithErrorCode>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(exception.ErrorCode, ErrorCode.BadRequest);
        Assert.Equal(exception.Message, ErrorMessages.PasswordMustContainLetterAndNumber);
    }


    private string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}