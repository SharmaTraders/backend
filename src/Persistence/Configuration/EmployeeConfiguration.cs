using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasKey(employee => employee.Id);
        builder.HasIndex(employee => employee.PhoneNumber)
            .IsUnique();
        builder.HasIndex(employee => employee.Email)
            .IsUnique();

        builder.OwnsMany(employee => employee.TimeRecords,
            timeRecordBuilder =>
            {
                timeRecordBuilder.HasKey(timeRecord => timeRecord.Id);
            });
    }
}