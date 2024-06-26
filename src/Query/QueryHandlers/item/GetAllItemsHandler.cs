﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.item;

namespace Query.QueryHandlers.item;

public class GetAllItemsHandler : IRequestHandler<GetAllItems.Query, GetAllItems.Answer> {

    private readonly SharmaTradersContext _context;

    public GetAllItemsHandler(SharmaTradersContext context) {
        _context = context;
    }

    public async Task<GetAllItems.Answer> Handle(GetAllItems.Query request, CancellationToken cancellationToken) {
        var items = await _context.Items
            .Select(i => new GetAllItems.ItemDto(
                i.Id.ToString(),
                i.Name ,
                Math.Round(i.CurrentStockAmount, 2),
                Math.Round(i.CurrentEstimatedStockValuePerKilo, 2)
            ))
            .ToListAsync(cancellationToken);

        return new GetAllItems.Answer(items);    }
}