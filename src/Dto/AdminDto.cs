namespace Dto;

public record AdminDto(string Id, string Email) : UserDto(Id, Email, "Admin");