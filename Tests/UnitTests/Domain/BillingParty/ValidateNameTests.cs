using Domain.billingParty;
using Domain.Entity;
using Domain.Repository;
using Domain.utils;
using Dto;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.BillingParty;

public class ValidateNameTests {

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public void ValidateName_WithValidBillingPartyName_Success(string validName) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Address = "Valid Address",
            Name = validName
        };
        Assert.Equal(validName, billingPartyEntity.Name);
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public async Task ValidateName_WithValidBillingPartyName_WhenNameIsNotUnique_Fails(string validName) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validName is **NOT** unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueNameAsync(validName, It.IsAny<Guid>())).ReturnsAsync(false);

        CreateBillingPartyRequest request = new CreateBillingPartyRequest(validName, "address", null, 0, null, null);

        // Act and Assert                   
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            billingPartyDomain.CreateBillingParty(request));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyNameAlreadyExists(validName), exception.Message);

    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyNames), MemberType = typeof(BillingPartyFactory))]
    public void ValidateName_WithInValidBillingPartyName_Fails(string invalidName) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Name = invalidName
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("name", StringComparison.OrdinalIgnoreCase));
    }
}