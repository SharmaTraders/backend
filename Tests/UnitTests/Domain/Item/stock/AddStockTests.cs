using Domain.Entity;

namespace UnitTests.Domain.Item.stock;

public class AddStockTests {
    [Fact]
    public void AddStock_AddsStockAndIncreasesCurrentStockAmount() {
        // Arrange
        var itemEntity = new ItemEntity {
            Name = "Test Name",
            CurrentStockAmount = 0,
            CurrentEstimatedStockValuePerKilo = 50,
        };
        var stock = new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = 10,
            ExpectedValuePerKilo = 50,
            EntryCategory = StockEntryCategory.Purchase,
        };
        // Act
        itemEntity.AddStock(stock);
        // Assert
        Assert.Equal(10, itemEntity.CurrentStockAmount);
        Assert.Equal(50, itemEntity.CurrentEstimatedStockValuePerKilo);
        Assert.NotNull(itemEntity.StockHistory);
        Assert.Single(itemEntity.StockHistory);
    }

    [Fact]
    public void ReduceStock_ReduceStockAndDecreasesCurrentStockAmount() {
        // Arrange
        var itemEntity = new ItemEntity {
            Name = "Test Name",
            CurrentStockAmount = 10,
            CurrentEstimatedStockValuePerKilo = 50,
        };
        var stock = new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = 10,
            ExpectedValuePerKilo = 50,
            EntryCategory = StockEntryCategory.Purchase,
        };
        // Act
        itemEntity.ReduceStock(stock);
        // Assert
        Assert.Equal(0, itemEntity.CurrentStockAmount);
        Assert.Equal(50, itemEntity.CurrentEstimatedStockValuePerKilo);
        Assert.NotNull(itemEntity.StockHistory);
        Assert.Single(itemEntity.StockHistory);
    }

    [Fact]
    public void ReduceStock_ThrowsExceptionWhenStockAmountIsLessThanWeight() {
        // Arrange
        var itemEntity = new ItemEntity {
            Name = "Test Name",
            CurrentStockAmount = 10,
            CurrentEstimatedStockValuePerKilo = 50,
        };
        var stock = new Stock {
            Date = new DateOnly(2021, 1, 1),
            Weight = 20,
            ExpectedValuePerKilo = 50,
            EntryCategory = StockEntryCategory.Purchase,
        };
        // Act
        var exception = Assert.Throws<DomainValidationException>(() => itemEntity.ReduceStock(stock));
        // Assert
        Assert.Equal("weight", exception.Type);
        Assert.Equal(ErrorMessages.StockReduceCannotBeMoreThanCurrentStock, exception.Message);
    }
}