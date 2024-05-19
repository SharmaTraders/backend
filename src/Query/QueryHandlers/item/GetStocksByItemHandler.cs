using Application.CommandHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.item;
using Tools;

namespace Query.QueryHandlers.item;

public class GetStocksByItemHandler : IRequestHandler<GetStocksByItem.Query, GetStocksByItem.Answer> {
    private readonly SharmaTradersContext _context;

    public GetStocksByItemHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<GetStocksByItem.Answer> Handle(GetStocksByItem.Query request, CancellationToken cancellationToken) {

        Guid itemId = GuidParser.ParseGuid(request.ItemId, "ItemId");

        IQueryable<Stock> queryable = _context.Stocks
            .Where(stock => stock.ItemEntityId == itemId);

        int totalItems = await queryable.CountAsync(cancellationToken);

        List<GetStocksByItem.StockDto> stocks =await queryable
            .OrderByDescending(stock => stock.Date)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(stock => new GetStocksByItem.StockDto(stock.Id.ToString(), stock.Weight, stock.Date.ToString(), stock.EntryCategory, stock.Remarks))
            .ToListAsync(cancellationToken);

        return new GetStocksByItem.Answer(stocks, totalItems, request.PageNumber, request.PageSize);

    }                                                                                             
}