using Application.CommandHandlers.item;
using CommandContracts.item;
using Domain.Repository;
using Moq;
using UnitTests.Fakes;

namespace UnitTests.Handlers;

public class ItemHandlersTests {


    [Fact]
    public async Task CannotCreateItem_WithDuplicateName() {

        var repositoryMock = new Mock<IItemRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        // When item name is not unique
        var uniqueItemNameChecker = new MockUniqueItemNameChecker() {
            Value = false
        };


        CreateItemHandler handler = new CreateItemHandler(repositoryMock.Object, unitOfWorkMock,uniqueItemNameChecker);

        var exception = await Assert.ThrowsAsync<DomainValidationException>(
              () =>
             handler.Handle(new CreateItemCommand.Request("Valid name", 10, 10),
                new CancellationToken()));

        Assert.Equal("itemname", exception.Type.ToLower());
        Assert.Equal(ErrorMessages.ItemNameAlreadyExists("Valid name"), exception.Message);

    }
}