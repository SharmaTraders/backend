using Data.Entities;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DatabaseContext : DbContext {

    public DbSet<AdminEntity> Admins { get; set; }


    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<AdminEntity>()
            .HasIndex(admin => admin.Email)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseExceptionProcessor();
        base.OnConfiguring(optionsBuilder);
    }
}