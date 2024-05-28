using Application.CommandHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.income;

namespace Query.QueryHandlers.Income;

public class
    GetIncomeByBillingPartyHandler : IRequestHandler<GetIncomeByBillingParty.Query, GetIncomeByBillingParty.Answer> {
    private readonly SharmaTradersContext _context;

    public GetIncomeByBillingPartyHandler(SharmaTradersContext context) {
        _context = context;
    }

    public async Task<GetIncomeByBillingParty.Answer> Handle(GetIncomeByBillingParty.Query request,
        CancellationToken cancellationToken) {
        Guid id = GuidParser.ParseGuid(request.BillingPartyId, "BillingPartyId");

        var queryable = _context.Incomes.Where(income => income.BillingPartyId == id);
        int totalCount = await queryable.CountAsync(cancellationToken);

        List<GetIncomeByBillingParty.IncomeDto> incomeDtos = await queryable.OrderByDescending(income => income.Date)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(income => new GetIncomeByBillingParty.IncomeDto(income.Id.ToString(),
                income.Date.ToString(),
                Math.Round(income.Amount, 2),
                income.Remarks))
            .ToListAsync(cancellationToken);
        return new GetIncomeByBillingParty.Answer(incomeDtos, totalCount, request.PageNumber, request.PageSize);
    }
}