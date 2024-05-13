using MediatR;

namespace CommandContracts.billingParty;

public static class CreateCommand {
    public record Request(
        string Name,
        string Address,
        string? PhoneNumber,
        double? OpeningBalance,
        string? Email,
        string? VatNumber) : IRequest<Response>;

    public record Response(string Id);
}