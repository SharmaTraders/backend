using Application.CommandHandlers.billingParty;
using CommandContracts.billingParty;
using Domain.Repository;
using Moq;
using UnitTests.Fakes;

namespace UnitTests.Handlers;

public class BillingPartyHandlersTests {
    [Fact]
    public async Task CannotCreate_BillingPartyWith_DuplicateName() {
        var repoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var uniqueBillingPartyNameChecker = new MockUniqueBillingPartyNameChecker() {
            Value = false
        };
        var uniqueBillingPartyEmailChecker = new MockUniqueBillingPartyEmailChecker() {
            Value = true
        };
        var uniqueBillingPartyVatNumberChecker = new MockUniqueBillingVatNumberChecker() {
            Value = true
        };
        CreateBillingPartyHandler handler = new CreateBillingPartyHandler(repoMock.Object,
            unitOfWorkMock,
            uniqueBillingPartyNameChecker,
            uniqueBillingPartyVatNumberChecker,
            uniqueBillingPartyEmailChecker);

        var request = new CreateCommand.Request("Valid name",
            "Valid address", 
            "1234567890", 
            10,
            "valid@valid.com",
            "ABCDEFGH");
        var exception = await Assert.ThrowsAsync<DomainValidationException>( () => handler.Handle(request, new CancellationToken()));
        Assert.Equal("name", exception.Type.ToLower());
        Assert.Equal(ErrorMessages.BillingPartyNameAlreadyExists("Valid name"), exception.Message);
    }

    [Fact]
    public async Task CannotCreate_BillingPartyWith_DuplicateEmail() {
        var repoMock = new Mock<IBillingPartyRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var uniqueBillingPartyNameChecker = new MockUniqueBillingPartyNameChecker() {
            Value = true
        };
        var uniqueBillingPartyEmailChecker = new MockUniqueBillingPartyEmailChecker() {
            Value = false
        };
        var uniqueBillingPartyVatNumberChecker = new MockUniqueBillingVatNumberChecker() {
            Value = true
        };
        CreateBillingPartyHandler handler = new CreateBillingPartyHandler(repoMock.Object,
            unitOfWorkMock,
            uniqueBillingPartyNameChecker,
            uniqueBillingPartyVatNumberChecker,
            uniqueBillingPartyEmailChecker);

        var request = new CreateCommand.Request("Valid name",
            "Valid address", 
            "1234567890", 
            10,
            "valid@valid.com",
            "ABCDEFGH");
        var exception = await Assert.ThrowsAsync<DomainValidationException>( () => handler.Handle(request, new CancellationToken()));
        Assert.Equal("email", exception.Type.ToLower());
        Assert.Equal(ErrorMessages.BillingPartyEmailAlreadyExists("valid@valid.com"), exception.Message);
    }

      [Fact]
        public async Task CannotCreate_BillingPartyWith_DuplicateVatNumber() {
            var repoMock = new Mock<IBillingPartyRepository>();
            var unitOfWorkMock = new MockUnitOfWork();
            var uniqueBillingPartyNameChecker = new MockUniqueBillingPartyNameChecker() {
                Value = true
            };
            var uniqueBillingPartyEmailChecker = new MockUniqueBillingPartyEmailChecker() {
                Value = true
            };
            var uniqueBillingPartyVatNumberChecker = new MockUniqueBillingVatNumberChecker() {
                Value = false
            };
            CreateBillingPartyHandler handler = new CreateBillingPartyHandler(repoMock.Object,
                unitOfWorkMock,
                uniqueBillingPartyNameChecker,
                uniqueBillingPartyVatNumberChecker,
                uniqueBillingPartyEmailChecker);
    
            var request = new CreateCommand.Request("Valid name",
                "Valid address", 
                "1234567890", 
                10,
                "valid@valid.com",
                "ABCDEFGH");
            var exception = await Assert.ThrowsAsync<DomainValidationException>( () => handler.Handle(request, new CancellationToken()));
            Assert.Equal("vatnumber", exception.Type.ToLower());
            Assert.Equal(ErrorMessages.BillingPartyVatNumberAlreadyExists("ABCDEFGH"), exception.Message);
        }
}