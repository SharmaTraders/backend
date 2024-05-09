using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class ItemConfiguration : IEntityTypeConfiguration<ItemEntity>{

    public void Configure(EntityTypeBuilder<ItemEntity> builder) {
        builder.HasKey(item => item.Id);
        builder.HasIndex(item => item.Name)
            .IsUnique();
    }
}