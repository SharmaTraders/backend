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

    public virtual DbSet<Item> Items { get; set; }

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

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Items_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
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
