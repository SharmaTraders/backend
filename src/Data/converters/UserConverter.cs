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


    public static AdminEntity ToEntity(AdminDto adminDto) {
        bool canBeParsed = Guid.TryParse(adminDto.Id, out Guid id);
        if (!canBeParsed) throw new ValidationException("Id",ErrorCode.BadRequest, ErrorMessages.IdInvalid);

        return new AdminEntity() {
            Id = id,
            Email = adminDto.Email,
            Password = adminDto.Password
        };
    }
}