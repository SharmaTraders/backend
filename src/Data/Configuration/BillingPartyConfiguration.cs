using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class BillingPartyConfiguration : IEntityTypeConfiguration<BillingPartyEntity> {

    public void Configure(EntityTypeBuilder<BillingPartyEntity> builder) {
        builder.HasKey(party => party.Id);
        builder.HasIndex(party => party.Name)
            .IsUnique();
        builder.HasIndex(party => party.VatNumber)
            .IsUnique();
        builder.HasIndex(party => party.Email)
            .IsUnique();
    }
}