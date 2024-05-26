using MediatR;

namespace CommandContracts.expense;

public static class RegisterExpenseCommand {
    public record Request(string Date, string? Category, double Amount, string? Remarks, string? BillingPartyId, string? EmployeeId)
        : IRequest<Answer>;

    public record Answer(string Id);
}