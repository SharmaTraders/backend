using Data.converters;
using Data.Entities;
using Domain.dao;
using Domain.utils;
using Dto;
using Dto.tools;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace Data.dao;

public class AuthenticationDao(DatabaseContext databaseContext) : IAuthenticationDao {
    public async Task<UserDto?> GetUserByEmail(string loginRequestEmail) {
        AdminEntity? adminEntity = await databaseContext.Admins.FirstOrDefaultAsync(
            entity => entity.Email.ToLower().Equals(loginRequestEmail.ToLower()));

        return UserConverter.ToEntity(adminEntity);
    }

    public async Task RegisterAdmin(AdminDto adminToRegister) {
        try {
            AdminEntity adminEntity = UserConverter.ToEntity(adminToRegister);
            await databaseContext.Admins.AddAsync(adminEntity);
            await databaseContext.SaveChangesAsync();
        }
        catch (Exception e) {
            if (e is UniqueConstraintException) {
                throw new ExceptionWithErrorCode(ErrorCode.Conflict, ErrorMessages.AdminWithEmailAlreadyExists);
            }
        }
    }
}