using MediatR;

namespace QueryContracts.income;

public static class GetIncomeByBillingParty {
    public record Query(string BillingPartyId, int PageNumber, int PageSize) : IRequest<Answer>;

    public record Answer(ICollection<IncomeDto> Incomes, int TotalCount, int PageNumber, int PageSize);

    public record IncomeDto(string Id, string Date, double Amount, string? Remarks);
}