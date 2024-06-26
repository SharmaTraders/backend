using Domain.Entity;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class WriteDatabaseContext : DbContext {
    public required DbSet<AdminEntity> Admins { get; init; }
    public required DbSet<ItemEntity> Items { get; init; }
    public required DbSet<BillingPartyEntity> BillingParties { get; init; }
    public required DbSet<PurchaseEntity> Purchases { get; init; }
    public required DbSet<SaleEntity> Sales { get; init; }
    public required DbSet<IncomeEntity> Incomes { get; init; }
    public required DbSet<EmployeeEntity> Employees { get; init; }
    public DbSet<ExpenseCategoryEntity> ExpenseCategories { get; set; }

    public DbSet<ExpenseEntity> Expenses { get; set; }


    public WriteDatabaseContext(DbContextOptions<WriteDatabaseContext> options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDatabaseContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseExceptionProcessor();
        base.OnConfiguring(optionsBuilder);
    }
}