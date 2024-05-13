using MediatR;

namespace CommandContracts.item;

public static class AddStockCommand {
    public record Request(
        string ItemId,
        double Weight,
        double ExpectedValuePerKilo,
        string Date,
        string? Remarks) : IRequest;


    
}