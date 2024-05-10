using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity> {
    public void Configure(EntityTypeBuilder<AdminEntity> builder) {
        builder.HasKey(admin => admin.Id);
        builder.HasIndex(admin => admin.Email)
            .IsUnique();
    }
}