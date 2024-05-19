using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategoryEntity> {

    public void Configure(EntityTypeBuilder<ExpenseCategoryEntity> builder) {
        builder.HasKey(expense => expense.Name);
    }
}