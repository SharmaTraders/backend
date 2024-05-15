using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class PurchaseConfiguration : IEntityTypeConfiguration<PurchaseEntity>
{
    
    public void Configure(EntityTypeBuilder<PurchaseEntity> builder)
    {
        builder.HasKey(purchase => purchase.Id);
        builder.HasOne(purchase => purchase.BillingParty)
            .WithMany();
        builder.OwnsMany(purchase => purchase.Purchases,
            purchaseBuilder => {
                purchaseBuilder.HasKey(purchase => purchase.Id);
                purchaseBuilder.HasOne(purchase => purchase.ItemEntity)
                    .WithMany();
            });
        
    }
}