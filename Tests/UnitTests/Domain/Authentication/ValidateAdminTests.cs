using Domain.auth;
using Domain.dao;
using Domain.utils;
using Dto;
using Moq;
using UnitTests.Factory;

namespace UnitTests.Domain.Authentication;

public class ValidateAdminTests {
    [Theory]
    [MemberData(nameof(UserFactory.GetValidEmails), MemberType = typeof(UserFactory))]
    public async Task ValidateAdmin_WithValidEmail_ReturnsUserDto(string email) {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var userDto = new AdminDto("id", email, HashPassword("password1"));

        authDaoMock.Setup(mock => mock.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(userDto);

        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto(email, "password1");

        // Act
        var result = await authenticationDomain.ValidateAdmin(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto, result);
        // Add more assertions based on your expected behavior
    }


    [Theory]
    [MemberData(nameof(UserFactory.GetInValidEmails), MemberType = typeof(UserFactory))]
    public async Task ValidateAdmin_WithInvalidEmail_ThrowsException(string email) {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto(email, "password1");

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => authenticationDomain.ValidateAdmin(loginRequest));

        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        Assert.False(authDaoMock.Invocations.Any());
    }

    [Theory]
    [MemberData(nameof(UserFactory.GetValidPasswords), MemberType = typeof(UserFactory))]
    public async Task ValidateAdmin_WithValidPassword_ReturnsUserDto(string password) {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var userDto = new AdminDto("id", "email@gmail.com", HashPassword(password));

        authDaoMock.Setup(mock => mock.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(userDto);

        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto(userDto.Email, password);

        // Act
        var result = await authenticationDomain.ValidateAdmin(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto, result);
        // Add more assertions based on your expected behavior
    }


    [Theory]
    [MemberData(nameof(UserFactory.GetInvalidPasswords), MemberType = typeof(UserFactory))]
    public async Task ValidateAdmin_WithInvalidPassword_ThrowsException(string password) {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var userDto = new AdminDto("id", "email@gmail.com", HashPassword(password));

        authDaoMock.Setup(mock => mock.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(userDto);

        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto(userDto.Email, password);

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => authenticationDomain.ValidateAdmin(loginRequest));

        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        Assert.False(authDaoMock.Invocations.Any());
    }


    [Fact]
    public async Task ValidateAdmin_WithValidCredentials_ReturnsUserDto() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        var userDto = new AdminDto("id", "email@email.com", HashPassword("password1"));

        authDaoMock.Setup(mock => mock.GetUserByEmail(It.IsAny<string>()))
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
        authDaoMock.Setup(mock => mock.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(null as AdminDto);

        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto("email@email.com", "password1");

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => authenticationDomain.ValidateAdmin(loginRequest));

        Assert.Equal(ErrorCode.NotFound, exception.ErrorCode);
        Assert.Equal(ErrorMessages.EmailDoesntExist, exception.Message);
    }

    [Fact]
    public async Task ValidateAdmin_WithIncorrectPassword_ThrowsException() {
        // Arrange
        var authDaoMock = new Mock<IAuthenticationDao>();
        AdminDto adminDto = new AdminDto("id", "email@email.com", HashPassword("password1"));

        authDaoMock.Setup(mock => mock.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(adminDto);

        var authenticationDomain = new AuthenticationDomain(authDaoMock.Object);
        var loginRequest = new LoginRequestDto("email@email.com", "incorrectPassword1");

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => authenticationDomain.ValidateAdmin(loginRequest));

        // Assert the exception properties
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.PasswordIncorrect, exception.Message);
    }

    private static string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}