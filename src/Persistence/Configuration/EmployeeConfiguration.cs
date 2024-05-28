using System.ComponentModel;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {
            builder.HasKey(employee => employee.Id);

            builder.HasIndex(employee => employee.PhoneNumber)
                .IsUnique();
            
            builder.HasIndex(employee => employee.Email)
                .IsUnique();
            
            builder.Property(employee => employee.Status)
                .HasConversion(
                    status => status.DisplayName,
                    dbValue => FromString(dbValue))
                .IsRequired();
            
            builder.OwnsMany(employee => employee.WorkShifts,
                workShiftRecordBuilder =>
                {
                    workShiftRecordBuilder.HasKey(workShift => workShift.Id);
                    workShiftRecordBuilder.Property(workShift => workShift.StartTime).IsRequired();
                    workShiftRecordBuilder.Property(workShift => workShift.EndTime).IsRequired();
                    workShiftRecordBuilder.HasIndex(workShift => workShift.Date);
                    workShiftRecordBuilder.Property(workShift => workShift.Date).IsRequired();
                    workShiftRecordBuilder.Property(workShift => workShift.BreakMinutes).IsRequired();
                });
            
            builder.OwnsMany(employee => employee.SalaryRecords,
                salaryRecordBuilder =>
                {
                    salaryRecordBuilder.HasKey(salaryRecord => salaryRecord.Id);
                    salaryRecordBuilder.Property(salaryRecord => salaryRecord.SalaryPerHour).IsRequired();
                    salaryRecordBuilder.Property(salaryRecord => salaryRecord.FromDate).IsRequired();
                    salaryRecordBuilder.Property(salaryRecord => salaryRecord.OvertimeSalaryPerHour).IsRequired();
                });
        }

        private static EmployeeStatusCategory FromString(string category) {
            return EmployeeStatusCategory.GetAll().FirstOrDefault(
                       e => e.DisplayName.Equals(category, StringComparison.OrdinalIgnoreCase)) ??
                   throw new InvalidEnumArgumentException("Invalid enum type");
        }
    }
}
