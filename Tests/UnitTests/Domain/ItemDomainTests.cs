using Domain.dao;
using Domain.item;
using Domain.utils;
using Dto;
using Moq;
using UnitTests.Factory;

namespace UnitTests.Domain;

public class ItemDomainTests {

    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]
    public async Task CreateItem_WithValidItemName_CallsItemDao(string itemName) {
        // Arrange
        var itemDaoMock = new Mock<IItemDao>();
        var itemDomain = new ItemDomain(itemDaoMock.Object);

        ItemDto itemDto = new(itemName);

        // Act
        await itemDomain.CreateItem(itemDto);

        // Assert that the dao is called.
        itemDaoMock.Verify(mock => mock.CreateItem(itemDto), Times.Once);

    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInValidItemNames), MemberType = typeof(ItemFactory))]
    public async Task CreateItem_WithInvalidItemName_ThrowsException(string itemName) {
        // Arrange
        var itemDaoMock = new Mock<IItemDao>();
        var itemDomain = new ItemDomain(itemDaoMock.Object);

        ItemDto itemDto = new(itemName);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => itemDomain.CreateItem(itemDto));

        Assert.Equal(ErrorCode.BadRequest, exception.ErrorCode);
        // Assert that the dao is never called
        Assert.False(itemDaoMock.Invocations.Any());
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]

    public async Task CreateItem_WithValidItemNameThatAlreadyExists_ThrowsException(string itemName) {
        // Arrange
        var itemDaoMock = new Mock<IItemDao>();
        var itemDomain = new ItemDomain(itemDaoMock.Object);

        ItemDto itemDto = new(itemName);

        itemDaoMock.Setup(mock => mock.GetItemByName(itemName))
            .ReturnsAsync(itemDto);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => itemDomain.CreateItem(itemDto));

        Assert.Equal(ErrorCode.Conflict, exception.ErrorCode);
        Assert.Equal(ErrorMessages.ItemNameAlreadyExists(itemName), exception.Message);
        // Assert that the dao is called to check if the item already exists.
        itemDaoMock.Verify(mock => mock.GetItemByName(itemName), Times.Once);
        // But the dao is never called to create the item.
        itemDaoMock.Verify(mock => mock.CreateItem(itemDto), Times.Never);
    }

    
    [Fact]
    public async Task CreateItem_WithValidItemNames_WithEmployeeToken_Fails() {
        // Todo this test needs to be done after implementing the employee login.
    }

}