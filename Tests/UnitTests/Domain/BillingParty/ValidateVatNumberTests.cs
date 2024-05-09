using Domain.billingParty;
using Domain.Entity;
using Domain.Repository;
using Dto;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.BillingParty;

public class ValidateVatNumberTests {
    
    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyVatNumber), MemberType = typeof(BillingPartyFactory))]

    public void ValidateVatNumber_WithValidBillingPartyVatNumber_Success(string validVatNumber) {
        // No exception is thrown
        BillingPartyEntity billingPartyEntity = new BillingPartyEntity {
            Address = "Valid Address",
            Name = "validName" ,
            VatNumber = validVatNumber
        };
        Assert.Equal(validVatNumber, billingPartyEntity.VatNumber);
    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetValidBillingPartyVatNumberNotEmpty), MemberType = typeof(BillingPartyFactory))]

    public async Task ValidateVatNumber_WithValidBillingPartyVatNumber_WhenVatNumberIsNotUnique_Fails(string validVatNumber) {
        // Arrange
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();   
        
        var billingPartyDomain = new BillingPartyDomain(billingPartyRepoMock.Object, unitOfWorkMock);
        // When the validVatNumber is **NOT** unique
        billingPartyRepoMock.Setup(mock => mock.IsUniqueVatNumberAsync(validVatNumber, It.IsAny<Guid>())).ReturnsAsync(false);
        billingPartyRepoMock.Setup(mock => mock.IsUniqueNameAsync(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);
        billingPartyRepoMock.Setup(mock => mock.IsUniqueEmailAsync(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);

        CreateBillingPartyRequest request = new CreateBillingPartyRequest("valid name", "valid address", null, 0, null, validVatNumber);

        // Act and Assert                   
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            billingPartyDomain.CreateBillingParty(request));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.BillingPartyVatNumberAlreadyExists(validVatNumber), exception.Message);

    }

    [Theory]
    [MemberData(nameof(BillingPartyFactory.GetInValidBillingPartyVatNumber), MemberType = typeof(BillingPartyFactory))]

    public void ValidateVatNumber_WithInValidBillingPartyVatNumber_Fails(string invalidVatNumber) {
        var exception = Assert.Throws<DomainValidationException>( ()  => new BillingPartyEntity {
            Address = "valid address",
            Name = "valid name",
            VatNumber = invalidVatNumber
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("vatnumber", StringComparison.OrdinalIgnoreCase));
    }

}