using Domain.billingParty;
using Domain.dao;
using Domain.utils;
using Moq;
using UnitTests.Factory;

namespace UnitTests.Domain.BillingParty;

public class ValidateVatNumberTests {
    
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyVatNumber), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateVatNumber_WithValidBillingPartyVatNumber_Success(string validVatNumber) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);
        // When the validVatNumber is unique
        billingPartyDaoMock.Setup(mock => mock.IsUniqueVatNumber(validVatNumber)).ReturnsAsync(true);

        // Act                    
        await billingPartyDomain.ValidateVatNumber(validVatNumber);

        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyVatNumberNotEmpty), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateVatNumber_WithValidBillingPartyVatNumber_WhenVatNumberIsNotUnique_Fails(string validVatNumber) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);
        // When the validVatNumber is **NOT** unique
        billingPartyDaoMock.Setup(mock => mock.IsUniqueVatNumber(validVatNumber)).ReturnsAsync(false);

        // Act and Assert                   
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() =>
            billingPartyDomain.ValidateVatNumber(validVatNumber));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyVatNumberAlreadyExists(validVatNumber), exception.Message);

    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyVatNumber), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateVatNumber_WithInValidBillingPartyVatNumber_Fails(string invalidVatNumber) {
        // Arrange
        var billingPartyDaoMock = new Mock<IBillingPartyDao>();
        var billingPartyDomain = new BillingPartyDomain(billingPartyDaoMock.Object);

        // Act and assert                    
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(() =>
            billingPartyDomain.ValidateVatNumber(invalidVatNumber));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        billingPartyDaoMock.Verify(mock => mock.IsUniqueVatNumber(invalidVatNumber), Times.Never);
    }

}