using Dto;

namespace Domain.auth;

public interface IAuthenticationDomain {
    Task<UserDto> ValidateAdmin(LoginRequestDto loginRequest);
    Task RegisterAdmin(RegisterAdminRequestDto registerAdminRequest);
}