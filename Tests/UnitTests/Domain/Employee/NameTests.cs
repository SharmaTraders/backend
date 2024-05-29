using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Employee;

public class NameTests
{
    [Theory]
    [MemberData(nameof(EmployeeFactory.GetValidEmployeeNames), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithValidName_CanBeCreated(string validName)
    {
        var employee = new EmployeeEntity
        {
            Name = validName,
            Address = "Valid Address",
            NormalDailyWorkingMinute = 480,
            Status = EmployeeStatusCategory.Active
        };
        Assert.Equal(validName, employee.Name);
    }

    [Theory]
    [MemberData(nameof(EmployeeFactory.GetInvalidEmployeeNames), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithInvalidName_CannotBeCreated(string invalidName)
    {
        var exception = Assert.Throws<DomainValidationException>(() => new EmployeeEntity
        {
            Name = invalidName,
            Address = "Valid Address",
            NormalDailyWorkingMinute = 480,
            Status = EmployeeStatusCategory.Active
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("Name", StringComparison.OrdinalIgnoreCase));
    }
}