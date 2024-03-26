using Dto;

namespace Domain.auth;

public interface IAuthenticationDomain {
    Task<UserDto> ValidateAdmin(LoginRequest loginRequest);
    Task RegisterAdmin(RegisterAdminRequest registerAdminRequest);
}