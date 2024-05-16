using Domain.Entity;

namespace Domain.DomainServices;

public static class AddIncomeService {

    public static void AddIncome(IncomeEntity incomeEntity) {
        BillingPartyEntity partyEntity = incomeEntity.BillingParty;
        // Income means we decrease the balance of the party, since they paid us
        double amountToUpdate = incomeEntity.Amount * -1;
        partyEntity.UpdateBalance(amountToUpdate);

    }
    
}