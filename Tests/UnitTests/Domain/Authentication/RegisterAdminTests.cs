using Domain.auth;
using Domain.dao;
using Domain.utils;
using Dto;
using Moq;

namespace UnitTests.Domain.Authentication;

public class RegisterAdminTests {
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
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.EmailInvalidFormat, exception.Message);
    }

    [Fact]
    public async Task RegisterAdmin_WithInvalidPassword_Short_ThrowsException() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var registerAdminRequest = new RegisterAdminRequestDto("valid@email.com", "short");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.PasswordBiggerThan5Characters, exception.Message);
    }

    [Fact]
    public async Task RegisterAdmin_WithInvalidPassword_NoLetterAndNumber_ThrowsException() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var registerAdminRequest = new RegisterAdminRequestDto("valid@email.com", "longWithoutNumber");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.PasswordMustContainLetterAndNumber, exception.Message);
    }

    [Fact]
    public async Task RegisterAdmin_WithAlreadyExistingEmail_ThrowsException() {
        // Arrange
        const string existingPassword = "somePassword12";
        const string existingEmail = "validEmail@gmail.com";
        AdminDto existingAdmin = new AdminDto("id", existingEmail, HashPassword(existingPassword));

        var authDaoMock = new Mock<IAuthenticationDao>();
        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var registerAdminRequest = new RegisterAdminRequestDto(existingEmail, "newPasswordNow12");

        authDaoMock.Setup(mock => mock.GetUserByEmail(existingEmail))
            .ReturnsAsync(existingAdmin);

        // Act and assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.EmailAlreadyExists, exception.Message);


    }


    private static string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

}