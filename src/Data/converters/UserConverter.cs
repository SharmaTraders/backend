using Data.Entities;
using Domain.utils;
using Dto;

namespace Data.converters;

public static class UserConverter {
    public static UserDto? ToDto(AdminEntity? adminEntity) {
        return adminEntity is null
            ? null
            : new AdminDto(adminEntity.Id.ToString(), adminEntity.Email, adminEntity.Password);
    }


    public static AdminEntity ToEntity(RegisterAdminRequest admin) {
        
        return new AdminEntity() {
            Id = Guid.NewGuid(),
            Email = admin.Email,
            Password = admin.Password
        };
    }
}