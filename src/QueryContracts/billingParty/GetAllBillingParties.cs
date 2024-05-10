using MediatR;

namespace QueryContracts.billingParty;

public static class GetAllBillingParties {
    public record Query() : IRequest<Answer>;

    public record Answer(ICollection<BillingPartyDto> Parties);


    public record BillingPartyDto(string Id,
        string Name,
        string Address,
        string? Email,
        string? PhoneNumber,
        double? Balance,
        string? VatNumber); 
}