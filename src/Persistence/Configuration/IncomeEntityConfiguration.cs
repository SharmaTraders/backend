using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class IncomeEntityConfiguration : IEntityTypeConfiguration<IncomeEntity> {

    public void Configure(EntityTypeBuilder<IncomeEntity> builder) {
        builder.HasKey(income => income.Id);
        builder.HasOne(income => income.BillingParty).WithMany();

    }
}