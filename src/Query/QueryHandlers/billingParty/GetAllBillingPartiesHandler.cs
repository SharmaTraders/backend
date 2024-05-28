using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.billingParty;

namespace Query.QueryHandlers.billingParty;

public class GetAllBillingPartiesHandler : IRequestHandler<GetAllBillingParties.Query, GetAllBillingParties.Answer> {

    private readonly SharmaTradersContext _context;

    public GetAllBillingPartiesHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<GetAllBillingParties.Answer> Handle(GetAllBillingParties.Query request, CancellationToken cancellationToken) {
        var parties = await _context.BillingParties
            .Select(p => new GetAllBillingParties.BillingPartyDto(
                p.Id.ToString(),
                p.Name,
                p.Address,
                p.Email,
                p.PhoneNumber,
                Math.Round(p.Balance, 2),
                p.VatNumber
            ))
            .ToListAsync(cancellationToken);

        return new GetAllBillingParties.Answer(parties);
    }
}