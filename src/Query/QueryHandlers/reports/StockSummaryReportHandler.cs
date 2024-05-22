using Application.CommandHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.reports;
using Tools;

namespace Query.QueryHandlers.reports;

public class StockSummaryReportHandler : IRequestHandler<StockSummaryReport.Query, StockSummaryReport.Answer> {
    private readonly SharmaTradersContext _context;

    public StockSummaryReportHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<StockSummaryReport.Answer> Handle(StockSummaryReport.Query request,
        CancellationToken cancellationToken) {

        DateOnly fromDate = DateParser.ParseDate(request.FromDate);
        DateOnly toDate = DateParser.ParseDate(request.ToDate);

        if (toDate < fromDate) {
            throw new DomainValidationException("date", ErrorCode.BadRequest, ErrorMessages.FromDateBeforeToDate);
        }


        List<StockSummaryReport.StockSummaryDto> summaryDtos = new List<StockSummaryReport.StockSummaryDto>();

        var allItems = await _context.Items
            .AsNoTracking().ToListAsync(cancellationToken);

        foreach (var item in allItems) {
            var saleLineItems = _context.SaleLineItems
                .Include(lineItem => lineItem.SaleEntity)
                .AsNoTracking()
                .Where(lineItem => lineItem.ItemEntityId == item.Id)
                .Where(lineItem => lineItem.SaleEntity.Date >= fromDate && lineItem.SaleEntity.Date <= toDate);

            // Its really funny, because if i try to not have this check, it will throw an exception
            // Because AverageAsync will throw an exception if the collection is empty - which is extremely stupid from EF core
            // It should just return null. But no, it throws an exception. So i have to check if the collection is empty

            double averageSalePrice = await saleLineItems.AnyAsync(cancellationToken)
                ?
                await saleLineItems.AverageAsync(lineItem => lineItem.Price, cancellationToken)
                :
                0;

            var purchaseLineItems = _context.PurchaseLineItems
                .Include(lineItem => lineItem.PurchaseEntity)
                .AsNoTracking()
                .Where(lineItem => lineItem.ItemEntityId == item.Id)
                .Where(lineItem => lineItem.PurchaseEntity.Date >= fromDate && lineItem.PurchaseEntity.Date <= toDate);
                ;

            double averagePurchasePrice = await purchaseLineItems.AnyAsync(cancellationToken)
                ?
                await purchaseLineItems.AverageAsync(lineItem => lineItem.Price, cancellationToken)
                :
                0;

            summaryDtos.Add(new StockSummaryReport.StockSummaryDto(
                item.Name,
                averageSalePrice,
                averagePurchasePrice,
                item.CurrentStockAmount,
                item.CurrentEstimatedStockValuePerKilo
            ));
        }

        return new StockSummaryReport.Answer(summaryDtos);

    }
}