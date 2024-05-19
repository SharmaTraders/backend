using Domain.Entity;

namespace Domain.DomainServices;

public static class RegisterIncomeService {

    public static void RegisterIncome(IncomeEntity incomeEntity) {
        BillingPartyEntity partyEntity = incomeEntity.BillingParty;
        // Income means we decrease the balance of the party, since they paid us
        double amountToUpdate = incomeEntity.Amount * -1;
        partyEntity.AddBalance(amountToUpdate);

    }
    
}