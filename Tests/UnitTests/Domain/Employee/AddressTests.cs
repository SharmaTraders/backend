using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Employee;

public class AddressTests
{
    [Theory]
    [MemberData(nameof(EmployeeFactory.GetValidEmployeeAddresses), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithValidAddress_CanBeCreated(string validAddress)
    {
        var employee = new EmployeeEntity
        {
            Name = "Valid Name",
            Address = validAddress,
            NormalDailyWorkingMinute = 480,
            Status = EmployeeStatusCategory.Active
        };
        Assert.Equal(validAddress, employee.Address);
    }

    [Theory]
    [MemberData(nameof(EmployeeFactory.GetInvalidEmployeeAddresses), MemberType = typeof(EmployeeFactory))]
    public void Employee_WithInvalidAddress_CannotBeCreated(string invalidAddress)
    {
        var exception = Assert.Throws<DomainValidationException>(() => new EmployeeEntity
        {
            Name = "Valid Name",
            Address = invalidAddress,
            NormalDailyWorkingMinute = 480,
            Status = EmployeeStatusCategory.Active
        });
        Assert.NotEmpty(exception.Message);
        Assert.True(exception.Type.Equals("Address", StringComparison.OrdinalIgnoreCase));
    }
}