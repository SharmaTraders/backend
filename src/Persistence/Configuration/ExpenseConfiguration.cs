using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class ExpenseConfiguration : IEntityTypeConfiguration<ExpenseEntity> {
    public void Configure(EntityTypeBuilder<ExpenseEntity> builder) {
        builder.HasKey(expense => expense.Id);
        builder.HasOne(expense => expense.Category).WithMany();
        builder.HasOne(expense => expense.BillingParty).WithMany()
            .IsRequired(false);

    }
}