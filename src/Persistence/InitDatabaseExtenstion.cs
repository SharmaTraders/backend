using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data;

public static class InitDatabaseExtenstion {
    public static async Task<WriteDatabaseContext> InitDatabase(this WriteDatabaseContext context) {
        // Insert an admin if not exist.
        if (!await context.Admins.AnyAsync()) {
            AdminEntity adminEntity = new AdminEntity() {
                Email = "admin@admin.com",
                Password = "admin123"
            };
            await context.Admins.AddAsync(adminEntity);
        }

        // Insert a cash billing party if not exist.
        bool cashBillingPartyExists =
            await context.BillingParties.AnyAsync(party => party.Name.ToLower().Equals("cash"));
        if (!cashBillingPartyExists) {
            BillingPartyEntity partyEntity = new BillingPartyEntity() {
                Name = "Cash",
                Address = "Cash"
            };
            await context.BillingParties.AddAsync(partyEntity);
        }


        // Insert a billingparty category if not exist.
        bool billingPartyCategoryExists =
            await context.ExpenseCategories.AnyAsync(category => category.Name.ToLower().Equals("billing party"));
        if (!billingPartyCategoryExists) {
            ExpenseCategoryEntity categoryEntity = new ExpenseCategoryEntity() {
                Name = "Billing Party"
            };
            await context.ExpenseCategories.AddAsync(categoryEntity);
        }

        await context.SaveChangesAsync();
        return context;
    }

}