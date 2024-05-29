using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Employee;

public class NormalDailyWorkingMinutesTests
{
    [Theory]
    [MemberData(nameof(EmployeeFactory.GetValidNormalDailyWorkingMinutes), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithValidNormalDailyWorkingMinute_CanBeCreated(int validMinutes)
    {
        var employee = new EmployeeEntity
        {
            Name = "Valid Name",
            Address = "Valid Address",
            NormalDailyWorkingMinute = validMinutes,
            Status = EmployeeStatusCategory.Active
        };
        Assert.Equal(validMinutes, employee.NormalDailyWorkingMinute);
    }

    [Theory]
    [MemberData(nameof(EmployeeFactory.GetInvalidNormalDailyWorkingMinutes), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithInvalidNormalDailyWorkingMinute_CannotBeCreated(int invalidMinutes)
    {
        var exception = Assert.Throws<DomainValidationException>(() => new EmployeeEntity
        {
            Name = "Valid Name",
            Address = "Valid Address",
            NormalDailyWorkingMinute = invalidMinutes,
            Status = EmployeeStatusCategory.Active
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("NormalDailyWorkingMinute", StringComparison.OrdinalIgnoreCase));
    }
}