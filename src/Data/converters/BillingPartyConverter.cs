using Data.Entities;
using Dto;

namespace Data.converters;

public static class BillingPartyConverter {

    public static BillingPartyEntity ToEntity(CreateBillingPartyRequest request) {
        return new() {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Balance = request.OpeningBalance ?? 0.0,
            VatNumber = request.VatNumber
        };
    }

    private static BillingPartyDto ToDto(BillingPartyEntity entity) {
        return new(
            entity.Id.ToString(),
            entity.Name,
            entity.Address,
            entity.Email,
            entity.PhoneNumber,
            entity.Balance,
            entity.VatNumber
        );
    }

    public static ICollection<BillingPartyDto> ToDtoList(ICollection<BillingPartyEntity> entities) {
        return entities.Select(ToDto).ToList();
    }
}