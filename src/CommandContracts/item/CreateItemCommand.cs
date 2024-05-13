using MediatR;

namespace CommandContracts.item;

public static class CreateItemCommand {
    public record Request(
        string Name,
        double? StockWeight,
        double? EstimatedValuePerKilo
    ) : IRequest<Response>;

    public record Response(string Id);
}