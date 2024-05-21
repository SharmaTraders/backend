using MediatR;

namespace QueryContracts.reports;

public static class StockSummaryReport {
    public record Query(): IRequest<Answer>;

    public record Answer(ICollection<StockSummaryDto> StockSummary);


    public record StockSummaryDto(
        string ItemName,
        double AverageSalePrice,
        double AveragePurchasePrice,
        double StockAmount,
        double StockValue);

}