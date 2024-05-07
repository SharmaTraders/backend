using Domain.billingParty;
using Domain.Repositories;
using Domain.utils;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.BillingParty;

public class ValidateVatNumberTests {
    
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyVatNumber), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateVatNumber_WithValidBillingPartyVatNumber_Success(string validVatNumber) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validVatNumber is unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueVatNumberAsync(validVatNumber)).ReturnsAsync(true);

        // Act                    
        await billingPartyDomain.ValidateVatNumber(validVatNumber);

        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyVatNumberNotEmpty), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateVatNumber_WithValidBillingPartyVatNumber_WhenVatNumberIsNotUnique_Fails(string validVatNumber) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();   
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validVatNumber is **NOT** unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueVatNumberAsync(validVatNumber)).ReturnsAsync(false);

        // Act and Assert                   
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            billingPartyDomain.ValidateVatNumber(validVatNumber));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyVatNumberAlreadyExists(validVatNumber), exception.Message);

    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyVatNumber), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateVatNumber_WithInValidBillingPartyVatNumber_Fails(string invalidVatNumber) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();   
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);

        // Act and assert                    
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            billingPartyDomain.ValidateVatNumber(invalidVatNumber));
        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        billingPartyRepoMock.Verify(mock => mock.IsUniqueVatNumberAsync(invalidVatNumber), Times.Never);
    }

}