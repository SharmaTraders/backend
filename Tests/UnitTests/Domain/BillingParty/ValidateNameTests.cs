using Domain.billingParty;
using Domain.dao;
using Domain.utils;
using Moq;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class ValidateNameTests {

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public async Task ValidateName_WithValidBillingPartyName_Success(string validName) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);
        // When the validName is unique
        billingPartyDaoMock.Setup(mock => mock.IsUniqueName(validName)).ReturnsAsync(true);

        // Act                    
        await billingPartyDomain.ValidateName(validName);

        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public async Task ValidateName_WithValidBillingPartyName_WhenNameIsNotUnique_Fails(string validName) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);
        // When the validName is **NOT** unique
        billingPartyDaoMock.Setup(mock => mock.IsUniqueName(validName)).ReturnsAsync(false);

        // Act and Assert                   
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() =>
            billingPartyDomain.ValidateName(validName));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyNameAlreadyExists(validName), exception.Message);

    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public async Task ValidateName_WithInValidBillingPartyName_Fails(string invalidName) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);

        // Act and assert                    
       ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() =>
           billingPartyDomain.ValidateName(invalidName));
       Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
       // Assert that the dao is never called
       billingPartyDaoMock.Verify(mock => mock.IsUniqueName(invalidName), Times.Never);
    }
}