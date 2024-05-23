using MediatR;

namespace QueryContracts.reports;

public static class AllTransactionsReport {

    public record Query(string DateFrom, string DateTo) : IRequest<Answer>;

    public record Answer(ICollection<TransactionDto> Transactions);

    public record TransactionDto(string Date, string? Remarks, string Category, double Amount, string Type);

}