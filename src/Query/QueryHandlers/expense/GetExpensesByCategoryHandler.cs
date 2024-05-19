using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Expense;

namespace Query.QueryHandlers.expense;

public class GetExpensesByCategoryHandler : IRequestHandler<GetExpenseByCategory.Query, GetExpenseByCategory.Answer> {

    private readonly SharmaTradersContext _context;

    public GetExpensesByCategoryHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<GetExpenseByCategory.Answer> Handle(GetExpenseByCategory.Query request, CancellationToken cancellationToken) {
        var queryable = _context.Expenses.Where(expense => expense.CategoryName.ToLower().Equals(request.Category));
        int totalCount = await queryable.CountAsync(cancellationToken);

        var expenses = queryable.OrderByDescending(expense => expense.Date)
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .Select(expense => new GetExpenseByCategory.ExpenseDto(expense.Date.ToString(), expense.Amount, expense.CategoryName, expense.Remarks))
            .ToList();

        return new GetExpenseByCategory.Answer(expenses, totalCount, request.PageNumber, request.PageSize);

    }
}