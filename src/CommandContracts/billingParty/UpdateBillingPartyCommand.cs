using MediatR;

namespace CommandContracts.billingParty;

public static class UpdateBillingPartyCommand {

    public record Request(
        string Id,
        string Name,
        string Address,
        string? PhoneNumber,
        string? Email,
        string? VatNumber) : IRequest;
    
}