using MediatR;

namespace CommandContracts.item;

public static class ReduceStockCommand {
    public record Request(
        string ItemId,
        double Weight,
        string Date) : IRequest;
}