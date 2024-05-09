using Domain.Entity;
using Domain.Repository;
using DomainEntry.billingParty;
using Dto;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.BillingParty;

public class ValidateEmailTests {
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyEmails), MemberType = typeof(BillingPartyFactory))]

    public void ValidateEmail_WithValidBillingPartyEmail_Success(string validEmail) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Email = validEmail,
            Address = "Valid Address",
            Name = "Test Name"
        };
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyEmailsNotEmpty), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateEmail_WithValidBillingPartyEmail_WhenEmailIsNotUnique_Fails(string validEmail) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validEmail is **NOT** unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueEmailAsync(validEmail, It.IsAny<Guid>())).ReturnsAsync(false);
        billingPartyRepoMock.Setup(mock => mock.IsUniqueNameAsync(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);
        billingPartyRepoMock.Setup(mock => mock.IsUniqueVatNumberAsync(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);


        CreateBillingPartyRequest request = new CreateBillingPartyRequest("name", "address", null, 0, validEmail, null);

        // Act and Assert                   
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            billingPartyDomain.CreateBillingParty(request));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyEmailAlreadyExists(validEmail), exception.Message);

    }


    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyEmails), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateEmail_WithInValidBillingPartyEmail_Fails(string invalidEmail) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Email = invalidEmail,
            Name = "Test Name"
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("email", StringComparison.OrdinalIgnoreCase));

    }

}