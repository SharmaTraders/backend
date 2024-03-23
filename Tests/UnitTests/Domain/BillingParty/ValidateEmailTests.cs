using Domain.billingParty;
using Domain.dao;
using Domain.utils;
using Moq;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class ValidateEmailTests {
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyEmails), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateEmail_WithValidBillingPartyEmail_Success(string validEmail) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);
        // When the validEmail is unique
        billingPartyDaoMock.Setup(mock => mock.IsUniqueEmail(validEmail)).ReturnsAsync(true);

        // Act                    
        await billingPartyDomain.ValidateEmail(validEmail);

        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyEmailsNotEmpty), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateEmail_WithValidBillingPartyEmail_WhenEmailIsNotUnique_Fails(string validEmail) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);
        // When the validEmail is **NOT** unique
        billingPartyDaoMock.Setup(mock => mock.IsUniqueEmail(validEmail)).ReturnsAsync(false);

        // Act and Assert                   
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() =>
            billingPartyDomain.ValidateEmail(validEmail));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyEmailAlreadyExists(validEmail), exception.Message);

    }


    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyEmails), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateEmail_WithInValidBillingPartyEmail_Fails(string invalidEmail) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);

        // Act and assert                    
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() =>
            billingPartyDomain.ValidateEmail(invalidEmail));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        billingPartyDaoMock.Verify(mock => mock.IsUniqueEmail(invalidEmail), Times.Never);
    }

}