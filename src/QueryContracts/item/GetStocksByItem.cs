using MediatR;

namespace QueryContracts.item;

public static class GetStocksByItem {

    public record Query(string ItemId, int PageNumber, int PageSize) : IRequest<Answer>;

    public record Answer(ICollection<StockDto> Stocks, int TotalPages, int PageNumber, int PageSize);

    public record StockDto(string Id, double WeightChange, string Date, string EntryCategory,  string? Remarks);

}