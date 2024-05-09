using Domain;
using Domain.auth;
using Domain.Entity;
using Domain.Repository;
using Dto;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.Authentication;

public class RegisterAdminTests {
    [Fact]
    public async Task RegisterAdmin_WithValidData_CallsDaoRegisterAdmin() {
        // Arrange
        var adminRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var authenticationDomain = new AuthenticationDomain(adminRepoMock.Object, unitOfWorkMock);
        var registerAdminRequest = new RegisterAdminRequest("valid@email.com", "validPassword123");

        // Act
        await authenticationDomain.RegisterAdmin(registerAdminRequest);

        // Assert
        adminRepoMock.Verify(mock => mock.AddAsync(It.IsAny<AdminEntity>()), Times.Once);
        Assert.Equal(1, unitOfWorkMock.CallCount);
    }

    [Fact]
    public async Task RegisterAdmin_WithInvalidEmail_ThrowsException() {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();

        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var registerAdminRequest = new RegisterAdminRequest("invalid", "validPassword123");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.EmailInvalidFormat, exception.Message);
        Assert.Equal(0, unitOfWorkMock.CallCount);
    }

    [Fact]
    public async Task RegisterAdmin_WithInvalidPassword_Short_ThrowsException() {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var registerAdminRequest = new RegisterAdminRequest("valid@email.com", "short");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.PasswordBiggerThan5Characters, exception.Message);
        Assert.Equal(0, unitOfWorkMock.CallCount);

    }

    [Fact]
    public async Task RegisterAdmin_WithInvalidPassword_NoLetterAndNumber_ThrowsException() {
        // Arrange
        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var registerAdminRequest = new RegisterAdminRequest("valid@email.com", "longWithoutNumber");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        Assert.Equal(ErrorMessages.PasswordMustContainLetterAndNumber, exception.Message);
        Assert.Equal(0, unitOfWorkMock.CallCount);

    }

    [Fact]
    public async Task RegisterAdmin_WithAlreadyExistingEmail_ThrowsException() {
        // Arrange
        const string existingPassword = "somePassword12";
        const string existingEmail = "validEmail@gmail.com";
        AdminEntity existingAdmin = UserFactory.GetValidAdminEntity();

        var authRepoMock = new Mock<IAdminRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var authenticationDomain = new AuthenticationDomain(authRepoMock.Object, unitOfWorkMock);
        var registerAdminRequest = new RegisterAdminRequest(existingEmail, "newPasswordNow12");

        authRepoMock.Setup(mock => mock.GetByEmailAsync(existingEmail))
            .ReturnsAsync(existingAdmin);

        // Act and assert
        var exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            authenticationDomain.RegisterAdmin(registerAdminRequest));
        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.EmailAlreadyExists, exception.Message);


    }


    private static string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

}