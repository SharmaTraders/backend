using Domain.Entity;

namespace Domain.DomainServices;

public static class RegisterExpenseService {

    public static void RegisterExpense(ExpenseEntity expenseEntity) {
        BillingPartyEntity? partyEntity = expenseEntity.BillingParty;

        // If no party is associated with the expense, we can't update the balance
        if (partyEntity is null) return;

        // Expense means we increase the balance of the party, since we paid them
        double amountToUpdate = expenseEntity.Amount;
        partyEntity.AddBalance(amountToUpdate);

    }
    
}