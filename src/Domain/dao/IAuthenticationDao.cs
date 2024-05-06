using Dto;

namespace Domain.dao;

public interface IAuthenticationDao {
    /*
     * Returns the user with the given email if it exists, otherwise null
     */
    Task<UserDto?> GetUserByEmail(string loginRequestEmail);
    Task RegisterAdmin(RegisterAdminRequest adminToRegister);
}