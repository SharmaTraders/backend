using Domain.common;

namespace Domain.Entity;

public class ItemEntity : IEntity<Guid> {
    private string _name;
    private double _currentStockAmount;
    private double _currentEstimatedStockValuePerKilo;
    public Guid Id { get; set; }

    public required double CurrentStockAmount {
        get => _currentStockAmount;
        set {
            ValidateStockAmount(value);
            _currentStockAmount = value;
        }
    }

    public required double CurrentEstimatedStockValuePerKilo {
        get => _currentEstimatedStockValuePerKilo;
        set {
            ValidateValuePerKilo(value);
            _currentEstimatedStockValuePerKilo = value;
        }
    }

    public ICollection<Stock> StockHistory { get; set; } = new List<Stock>();


    public required string Name {
        get => _name;
        set {
            ValidateName(value);
            _name = value;
        }
    }

    public void AddStock(Stock stock) {
        StockHistory.Add(stock);
        CurrentStockAmount += Math.Round(stock.Weight, 2);
        CurrentEstimatedStockValuePerKilo = stock.ExpectedValuePerKilo != 0
            ? stock.ExpectedValuePerKilo
            : CurrentEstimatedStockValuePerKilo;
    }

    public void ReduceStock(Stock stock) {
        if (_currentStockAmount < stock.Weight) {
            throw new DomainValidationException("weight", ErrorCode.BadRequest, ErrorMessages.StockReduceCannotBeMoreThanCurrentStock);
            
        }

        StockHistory.Add(stock);
        CurrentStockAmount -= Math.Round(stock.Weight, 2);
    }


    private static void ValidateName(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("ItemName", ErrorCode.BadRequest, ErrorMessages.ItemNameIsRequired);
        }

        value = value.Trim();
        if (value.Length is < 3 or > 20) {
            throw new DomainValidationException("ItemName", ErrorCode.BadRequest, ErrorMessages.ItemNameLength);
        }
    }


    private static void ValidateStockAmount(double value) {
        if (value < 0) {
            throw new DomainValidationException("StockAmount", ErrorCode.BadRequest,
                ErrorMessages.StockAmountCannotBeNegative);
        }
    }

    private static void ValidateValuePerKilo(double value) {
        if (value < 0) {
            throw new DomainValidationException("ValuePerKilo", ErrorCode.BadRequest,
                ErrorMessages.StockValuePerKiloCannotBeNegative);
        }
    }
}