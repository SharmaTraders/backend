using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class SaleConfiguration : IEntityTypeConfiguration<SaleEntity>
{
    public void Configure(EntityTypeBuilder<SaleEntity> builder)
    {
            builder.HasKey(sale => sale.Id);
            builder.HasOne(sale => sale.BillingParty)
                .WithMany();
            builder.OwnsMany(sale => sale.Sales,
                saleLineItemBuilder => {
                    saleLineItemBuilder.HasKey(saleLineItem => saleLineItem.Id);
                    saleLineItemBuilder.HasOne(saleLineItem => saleLineItem.ItemEntity)
                        .WithMany();
                });
        
    }
}