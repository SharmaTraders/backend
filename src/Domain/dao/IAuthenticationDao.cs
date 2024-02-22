using Dto;

namespace Domain.dao;

public interface IAuthenticationDao {
    Task<UserDto?> GetOrDefaultUserByEmail(string loginRequestEmail);
    Task RegisterAdmin(AdminDto adminToRegister);
}