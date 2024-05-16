using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Query;

public partial class SharmaTradersContext : DbContext
{
    public SharmaTradersContext()
    {
    }

    public SharmaTradersContext(DbContextOptions<SharmaTradersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<BillingParty> BillingParties { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseLineItem> PurchaseLineItems { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Admins_Email").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<BillingParty>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_BillingParties_Email").IsUnique();

            entity.HasIndex(e => e.Name, "IX_BillingParties_Name").IsUnique();

            entity.HasIndex(e => e.VatNumber, "IX_BillingParties_VatNumber").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasIndex(e => e.BillingPartyId, "IX_Incomes_BillingPartyId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.BillingParty).WithMany(p => p.Incomes).HasForeignKey(d => d.BillingPartyId);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Items_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasIndex(e => e.BillingPartyId, "IX_Purchases_BillingPartyId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.BillingParty).WithMany(p => p.Purchases).HasForeignKey(d => d.BillingPartyId);
        });

        modelBuilder.Entity<PurchaseLineItem>(entity =>
        {
            entity.ToTable("PurchaseLineItem");

            entity.HasIndex(e => e.ItemEntityId, "IX_PurchaseLineItem_ItemEntityId");

            entity.HasIndex(e => e.PurchaseEntityId, "IX_PurchaseLineItem_PurchaseEntityId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ItemEntity).WithMany(p => p.PurchaseLineItems).HasForeignKey(d => d.ItemEntityId);

            entity.HasOne(d => d.PurchaseEntity).WithMany(p => p.PurchaseLineItems).HasForeignKey(d => d.PurchaseEntityId);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("Stock");

            entity.HasIndex(e => e.ItemEntityId, "IX_Stock_ItemEntityId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ItemEntity).WithMany(p => p.Stocks).HasForeignKey(d => d.ItemEntityId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
