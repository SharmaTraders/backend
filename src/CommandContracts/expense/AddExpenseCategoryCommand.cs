using MediatR;

namespace CommandContracts.expense;

public static class AddExpenseCategoryCommand {

    public record Request(string Name) : IRequest;
    
}