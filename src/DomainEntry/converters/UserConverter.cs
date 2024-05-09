using Domain.Entity;
using Dto;

namespace DomainEntry.converters;

public static class UserConverter {
    public static UserDto? ToDto(AdminEntity? adminEntity) {
        return adminEntity is null
            ? null
            : new AdminDto(adminEntity.Id.ToString(), adminEntity.Email);
    }


    public static AdminEntity ToEntity(RegisterAdminRequest admin) {
        return new AdminEntity() {
            Id = Guid.NewGuid(),
            Email = admin.Email,
            Password = admin.Password
        };
    }
}