using Data.converters;
using Data.Entities;
using Domain.dao;
using Dto;
using Microsoft.EntityFrameworkCore;

namespace Data.dao;

public class AuthenticationDao : IAuthenticationDao {
    private readonly DatabaseContext _databaseContext;

    public AuthenticationDao(DatabaseContext databaseContext) {
        this._databaseContext = databaseContext;
    }

    public async Task<UserDto?> GetUserByEmail(string loginRequestEmail) {
        AdminEntity? adminEntity = await _databaseContext.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(
            entity => entity.Email.ToLower().Equals(loginRequestEmail.ToLower()));

        return UserConverter.ToDto(adminEntity);
    }

    public async Task RegisterAdmin(RegisterAdminRequest adminToRegister) {
        AdminEntity adminEntity = UserConverter.ToEntity(adminToRegister);
        await _databaseContext.Admins
            .AddAsync(adminEntity);
        await _databaseContext.SaveChangesAsync();
    }
}