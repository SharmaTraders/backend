using MediatR;

namespace QueryContracts.Expense;

public static class GetExpenseByCategory {
    public record Query(string Category, int PageNumber , int PageSize) : IRequest<Answer>;

    public record Answer(ICollection<ExpenseDto> Expenses, int TotalCount, int PageNumber, int PageSize);

    public record ExpenseDto(string Date, double Amount, string CategoryName, string? Remarks);
}