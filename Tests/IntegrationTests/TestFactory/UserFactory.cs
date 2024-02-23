using Dto;
using IntegrationTests.util;

namespace IntegrationTests.TestFactory;

public static class UserFactory {

    public static AdminDto GetValidAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "admin@admin.com", "somePassword1234");

    public static AdminDto GetInvalidAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "badEmail", "badPassword");

    public static AdminDto GetInvalidEmailAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "badEmail", "goodPassword1"); 

    public static AdminDto GetInvalidPasswordLessThan5CharsAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "goodemail@email.com", "bad");

    public static AdminDto GetInvalidPasswordLessThanNoLettersAndNumbersAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "goodemail@email.com", "badButBigger");

    public static EmployeeDto GetValidEmployee() =>
        new EmployeeDto(Guid.NewGuid().ToString(), "employee@employee.com", "somePassword1234", "test user", "asdfsf",
            "12345678", "Active");

    public static EmployeeDto GetInvalidEmployee() =>
        new EmployeeDto(Guid.NewGuid().ToString(), "badEmail", "badPassword", "test user", "asdfsf",
            "12345678", "Active");


    public static string GetValidAdminJwtToken() =>
    JwtUtil.GenerateJwt(GetValidAdmin());

    public static string GetValidEmployeeJwtToken() =>
        JwtUtil.GenerateJwt(GetValidEmployee());


}