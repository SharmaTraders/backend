using MediatR;

namespace QueryContracts.item;

public static class GetAllItems {
    public record Query : IRequest<Answer>;

    public record Answer(ICollection<ItemDto> Items);
    public record ItemDto(string Id, string Name,double StockWeight, double EstimatedPricePerKilo);
}