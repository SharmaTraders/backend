using MediatR;

namespace CommandContracts.income;

public static class RegisterIncomeCommand {
    public record Request(string Date, string BillingPartyId, double Amount, string? Remarks) : IRequest<Answer>;

    public record Answer(string Id);

}