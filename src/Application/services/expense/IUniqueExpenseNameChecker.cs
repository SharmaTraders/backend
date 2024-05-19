namespace Application.services.expense;

public interface IUniqueExpenseNameChecker {
    Task<bool> IsUniqueAsync(string name);

}