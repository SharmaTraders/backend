using Domain.Entity;
using Domain.Repository;
using DomainEntry.item;
using Dto;
using Moq;
using UnitTests.Factory;
using UnitTests.Fakes;

namespace UnitTests.Domain.Item;

public class CreateItemTests {

    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]
    public async Task CreateItem_WithValidItemName_CallsItemDao(string itemName) {
        // Arrange
        var itemRepoMock = new Mock<IItemRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var itemDomain = new ItemDomain(itemRepoMock.Object, unitOfWorkMock);

        CreateItemRequest createItemRequest = new(itemName);

        // Act
        await itemDomain.CreateItem(createItemRequest);

        // Assert that the dao is called.
        itemRepoMock.Verify(mock => mock.AddAsync(It.IsAny<ItemEntity>()), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInValidItemNames), MemberType = typeof(ItemFactory))]
    public async Task CreateItem_WithInvalidItemName_ThrowsException(string itemName) {
        // Arrange
        var itemRepoMock = new Mock<IItemRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        
        var itemDomain = new ItemDomain(itemRepoMock.Object, unitOfWorkMock);

        CreateItemRequest createItemRequest = new(itemName);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainValidationException>(() => itemDomain.CreateItem(createItemRequest));

        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        Assert.False(itemRepoMock.Invocations.Any());
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]
    public async Task CreateItem_WithValidItemNameThatAlreadyExists_ThrowsException(string itemName) {
        // Arrange
        var itemRepoMock = new Mock<IItemRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        
        var itemDomain = new ItemDomain(itemRepoMock.Object, unitOfWorkMock);

        ItemEntity entity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = itemName
        };

        itemRepoMock.Setup(mock => mock.GetByNameAsync(itemName))
            .ReturnsAsync(entity);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainValidationException>(() => itemDomain.CreateItem(new CreateItemRequest(itemName)));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.ItemNameAlreadyExists(itemName), exception.Message);
        // Assert that the dao is called to check if the item already exists.
        itemRepoMock.Verify(mock => mock.GetByNameAsync(itemName), Times.Once);
        // But the dao is never called to create the item.
        itemRepoMock.Verify(mock => mock.AddAsync(It.IsAny<ItemEntity>()), Times.Never);
    }


    [Fact]
    public async Task CreateItem_WithValidItemNames_WithEmployeeToken_Fails() {
        // Todo this test needs to be done after implementing the employee login.
    }
}