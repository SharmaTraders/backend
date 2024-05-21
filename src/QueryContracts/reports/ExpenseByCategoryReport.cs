using MediatR;

namespace QueryContracts.reports;

public static class ExpenseByCategoryReport {
    
    public record Query(string? DateFrom, string? DateTo) : IRequest<Answer>;

    public record Answer(ICollection<ExpenseByCategoryDto> Expenses);

    public record ExpenseByCategoryDto(string Category, double Amount);
}