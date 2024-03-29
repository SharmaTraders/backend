using System.Text.RegularExpressions;
using Dto.tools;

namespace Dto;

public abstract record UserDto(
    string? Id, // This is Guid but for simplicity on data mapping from json to object, I use string
    string Email,
    string Password,
    string Role);