namespace Dto;

public record EmployeeDto(
    string Id,
    string Email,
    string Password,
    string FullName,
    string Address,
    string PhoneNumber,
    string Status)
    : UserDto(Id, Email, Password, "Employee");