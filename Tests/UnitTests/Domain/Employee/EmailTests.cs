using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Employee;

public class EmailTests
{
    [Theory]
    [MemberData(nameof(EmployeeFactory.GetValidEmployeeEmails), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithValidEmail_CanBeCreated(string validEmail)
    {
        var employee = new EmployeeEntity
        {
            Name = "Valid Name",
            Address = "Valid Address",
            Email = validEmail,
            NormalDailyWorkingMinute = 480,
            Status = EmployeeStatusCategory.Active
        };

        Assert.Equal(string.IsNullOrEmpty(validEmail) ? null : validEmail, employee.Email);
    }


    [Theory]
    [MemberData(nameof(EmployeeFactory.GetInvalidEmployeeEmails), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithInvalidEmail_CannotBeCreated(string invalidEmail)
    {
        var exception = Assert.Throws<DomainValidationException>(() => new EmployeeEntity
        {
            Name = "Valid Name",
            Address = "Valid Address",
            Email = invalidEmail,
            NormalDailyWorkingMinute = 480,
            Status = EmployeeStatusCategory.Active
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("Email", StringComparison.OrdinalIgnoreCase));
    }
}