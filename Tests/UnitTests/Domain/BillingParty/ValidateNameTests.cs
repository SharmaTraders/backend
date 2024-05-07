using Domain.billingParty;
using Domain.Repositories;
using Domain.utils;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.BillingParty;

public class ValidateNameTests {

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public async Task ValidateName_WithValidBillingPartyName_Success(string validName) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validName is unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueNameAsync(validName)).ReturnsAsync(true);

        // Act                    
        await billingPartyDomain.ValidateName(validName);

        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public async Task ValidateName_WithValidBillingPartyName_WhenNameIsNotUnique_Fails(string validName) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validName is **NOT** unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueNameAsync(validName)).ReturnsAsync(false);

        // Act and Assert                   
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            billingPartyDomain.ValidateName(validName));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyNameAlreadyExists(validName), exception.Message);

    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public async Task ValidateName_WithInValidBillingPartyName_Fails(string invalidName) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();   
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);

        // Act and assert                    
       DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
           billingPartyDomain.ValidateName(invalidName));
       Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
       // Assert that the dao is never called
       billingPartyRepoMock.Verify(mock => mock.IsUniqueNameAsync(invalidName), Times.Never);
    }
}