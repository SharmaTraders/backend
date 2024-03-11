namespace Dto;

public record AdminDto(string Id, string Email, string Password) : UserDto(Id, Email, Password, "Admin");