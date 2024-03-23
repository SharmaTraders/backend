using Data.Entities;
using Dto;

namespace Data.converters;

public static class BillingPartyConverter {

    public static BillingPartyEntity ToEntity(CreateBillingPartyRequestDto request) {
        return new() {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Balance = request.OpeningBalance,
            VatNumber = request.VatNumber
        };
    }
}