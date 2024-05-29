using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Employee;

public class PhoneNumberTests
{
    [Theory]
    [MemberData(nameof(EmployeeFactory.GetValidEmployeePhoneNumbers), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithValidPhoneNumber_CanBeCreated(string validPhoneNumber)
    {
        var employee = new EmployeeEntity
        {
            Name = "Valid Name",
            Address = "Valid Address",
            PhoneNumber = validPhoneNumber,
            NormalDailyWorkingMinute = 480,
            Status = EmployeeStatusCategory.Active
        };

        Assert.Equal(string.IsNullOrEmpty(validPhoneNumber) ? null : validPhoneNumber, employee.PhoneNumber);
    }


    [Theory]
    [MemberData(nameof(EmployeeFactory.GetInvalidEmployeePhoneNumbers), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithInvalidPhoneNumber_CannotBeCreated(string invalidPhoneNumber)
    {
        var exception = Assert.Throws<DomainValidationException>(() => new EmployeeEntity
        {
            Name = "Valid Name",
            Address = "Valid Address",
            PhoneNumber = invalidPhoneNumber,
            NormalDailyWorkingMinute = 480,
            Status = EmployeeStatusCategory.Active
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("PhoneNumber", StringComparison.OrdinalIgnoreCase));
    }
}