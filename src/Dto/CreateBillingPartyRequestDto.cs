namespace Dto;

public record CreateBillingPartyRequestDto(
    string Name,
    string Address,
    string? PhoneNumber,
    double? OpeningBalance,
    string? Email,
    string? VatNumber) {
}