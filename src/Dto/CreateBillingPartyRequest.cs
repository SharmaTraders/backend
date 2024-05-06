namespace Dto;

public record CreateBillingPartyRequest(
    string Name,
    string Address,
    string? PhoneNumber,
    double? OpeningBalance,
    string? Email,
    string? VatNumber) {
}