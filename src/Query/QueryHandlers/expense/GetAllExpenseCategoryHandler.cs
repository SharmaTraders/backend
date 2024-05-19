using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Expense;

namespace Query.QueryHandlers.expense;

public class GetAllExpenseCategoryHandler : IRequestHandler<GetAllExpenseCategory.Query, GetAllExpenseCategory.Answer> {
    private readonly SharmaTradersContext _context;

    public GetAllExpenseCategoryHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<GetAllExpenseCategory.Answer> Handle(GetAllExpenseCategory.Query request,
        CancellationToken cancellationToken) {
        List<string> categories = await _context.ExpenseCategories.Select(category => category.Name)
            .ToListAsync(cancellationToken);
        return new GetAllExpenseCategory.Answer(categories);
    }
}