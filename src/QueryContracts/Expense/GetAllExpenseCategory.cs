using MediatR;

namespace QueryContracts.Expense;

public static class GetAllExpenseCategory {
    public record Query() : IRequest<Answer>;

    public record Answer(ICollection<string> Categories);
}