using System.ComponentModel;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class ItemConfiguration : IEntityTypeConfiguration<ItemEntity> {
    public void Configure(EntityTypeBuilder<ItemEntity> builder) {
        builder.HasKey(item => item.Id);
        builder.HasIndex(item => item.Name)
            .IsUnique();

        builder.OwnsMany(item => item.StockHistory,
            stockBuilder => {
                stockBuilder.HasKey(stock => stock.Id);
                stockBuilder.Property(stock => stock.EntryCategory)
                    .HasConversion(
                        myValue => myValue.DisplayName,
                        dbValue => FromString(dbValue));

            });
    }

    private static StockEntryCategory FromString(string category) {
        return StockEntryCategory.GetAll().FirstOrDefault(
                   e => e.DisplayName.Equals(category, StringComparison.OrdinalIgnoreCase)) ??
               throw new InvalidEnumArgumentException("Invalid enum type");
    }
}