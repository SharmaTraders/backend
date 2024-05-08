using Domain.auth;
using Domain.Entity;
using Domain.Repository;
using Domain.utils;
using Dto;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.Authentication;

public class LoginAdminTests {
    [Theory]
    [MemberData(nameof(UserFactory.GetValidEmails), MemberType = typeof(UserFactory))]
    public async Task LoginAdmin_WithValidEmail_ReturnsUserDto(string email) {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var userEntity = new AdminEntity() {
            Id = Guid.Empty,
            Email = email,
            Password = "password1"
        };

        authRepoMock.Setup(mock => mock.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(userEntity);

        var unitOfWorkMock = new MockUnitOfWork();
        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var loginRequest = new LoginRequest(email, "password1");

        // Act
        var result = await authenticationDomain.LoginAdmin(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userEntity.Email, result.Email);
        // Add more assertions based on your expected behavior
    }


    [Theory]
    [MemberData(nameof(UserFactory.GetInValidEmails), MemberType = typeof(UserFactory))]
    public async Task LoginAdmin_WithInvalidEmail_ThrowsException(string email) {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();

        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var loginRequest = new LoginRequest(email, "password1");

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<DomainValidationException>(() => authenticationDomain.LoginAdmin(loginRequest));

        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        Assert.False(authRepoMock.Invocations.Any());
    }

    [Theory]
    [MemberData(nameof(UserFactory.GetValidPasswords), MemberType = typeof(UserFactory))]
    public async Task LoginAdmin_WithValidPassword_ReturnsUserDto(string password) {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();

        var userEntity = new AdminEntity() {
            Id = Guid.Empty,
            Email = "someValidEmail@gmail.com",
            Password =password
        };

        authRepoMock.Setup(mock => mock.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(userEntity);

        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var loginRequest = new LoginRequest(userEntity.Email, password);

        // Act
        var result = await authenticationDomain.LoginAdmin(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userEntity.Email, result.Email);
        // Add more assertions based on your expected behavior
    }


    [Theory]
    [MemberData(nameof(UserFactory.GetInvalidPasswords), MemberType = typeof(UserFactory))]
    public async Task LoginAdmin_WithInvalidPassword_ThrowsException(string password) {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        
        var user = UserFactory.GetValidAdminEntity();

        authRepoMock.Setup(mock => mock.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var loginRequest = new LoginRequest(user.Email, password);

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<DomainValidationException>(() => authenticationDomain.LoginAdmin(loginRequest));

        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        Assert.False(authRepoMock.Invocations.Any());
    }


    [Fact]
    public async Task LoginAdmin_WithValidCredentials_ReturnsUserDto() {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();

        var userEntity = new AdminEntity() {
            Id = Guid.Empty,
            Email = "someValidEmail@gmail.com",
            Password = "password1"
        };

        authRepoMock.Setup(mock => mock.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(userEntity);

        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var loginRequest = new LoginRequest("email@email.com", "password1");

        // Act
        var result = await authenticationDomain.LoginAdmin(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userEntity.Email, result.Email);
        // Add more assertions based on your expected behavior
    }

    [Fact]
    public async Task LoginAdmin_WithNonexistentEmail_ThrowsException() {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        
        authRepoMock.Setup(mock => mock.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(null as AdminEntity);

        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var loginRequest = new LoginRequest("email@email.com", "password1");

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<DomainValidationException>(() => authenticationDomain.LoginAdmin(loginRequest));

        Assert.Equal(ErrorCode.NotFound, exception.ErrorCode);
        Assert.Equal(ErrorMessages.EmailDoesntExist, exception.Message);
    }

    [Fact]
    public async Task LoginAdmin_WithIncorrectPassword_ThrowsException() {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();

        var user = UserFactory.GetValidAdminEntity();

        authRepoMock.Setup(mock => mock.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var loginRequest = new LoginRequest("email@email.com", "incorrectPassword1");

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<DomainValidationException>(() => authenticationDomain.LoginAdmin(loginRequest));

        // Assert the exception properties
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.PasswordIncorrect, exception.Message);
    }
}