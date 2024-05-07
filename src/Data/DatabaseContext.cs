using Domain.Entity;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DatabaseContext : DbContext {

    public required DbSet<AdminEntity> Admins { get; init; }
    public required DbSet<ItemEntity> Items { get; init; }
    public required DbSet<BillingPartyEntity> BillingParties { get; init; }


    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<AdminEntity>()
            .HasIndex(admin => admin.Email)
            .IsUnique();
        modelBuilder.Entity<BillingPartyEntity>()
            .HasIndex(party => party.Name)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseExceptionProcessor();
        base.OnConfiguring(optionsBuilder);
    }
}