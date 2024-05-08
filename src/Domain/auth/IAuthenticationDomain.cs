using Dto;

namespace Domain.auth;

public interface IAuthenticationDomain {
    Task<UserDto> LoginAdmin(LoginRequest loginRequest);
    Task RegisterAdmin(RegisterAdminRequest registerAdminRequest);
}