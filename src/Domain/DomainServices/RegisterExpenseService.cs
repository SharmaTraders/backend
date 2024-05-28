using Domain.Entity;

namespace Domain.DomainServices;

public static class RegisterExpenseService {
    public static void RegisterExpense(ExpenseEntity expenseEntity) {
        BillingPartyEntity? partyEntity = expenseEntity.BillingParty;
        EmployeeEntity? employeeEntity = expenseEntity.Employee;

        // If no party is associated with the expense or salary, we can't update the balance
        if (partyEntity is null && employeeEntity is null) return;

        double amountToUpdate = expenseEntity.Amount;
        if (partyEntity is not null) {
            // Expense means we increase the balance of the party, since we paid them
            partyEntity.AddBalance(amountToUpdate);
            return;
        }

        // Expense means we decrease the balance of the employee, since we paid them
        // Note that this is different from the party, because in this case positive balance means we owe them
        // While in the case of party, positive balance means they owe us
        employeeEntity?.AddBalance(amountToUpdate * -1);
    }
}