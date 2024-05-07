using Domain.billingParty;
using Domain.Repositories;
using Domain.utils;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.BillingParty;

public class ValidateEmailTests {
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyEmails), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateEmail_WithValidBillingPartyEmail_Success(string validEmail) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validEmail is unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueEmailAsync(validEmail)).ReturnsAsync(true);

        // Act                    
        await billingPartyDomain.ValidateEmail(validEmail);

        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyEmailsNotEmpty), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateEmail_WithValidBillingPartyEmail_WhenEmailIsNotUnique_Fails(string validEmail) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validEmail is **NOT** unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueEmailAsync(validEmail)).ReturnsAsync(false);

        // Act and Assert                   
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            billingPartyDomain.ValidateEmail(validEmail));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyEmailAlreadyExists(validEmail), exception.Message);

    }


    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyEmails), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateEmail_WithInValidBillingPartyEmail_Fails(string invalidEmail) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();  
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);

        // Act and assert                    
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            billingPartyDomain.ValidateEmail(invalidEmail));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        billingPartyRepoMock.Verify(mock => mock.IsUniqueEmailAsync(invalidEmail), Times.Never);
    }

}