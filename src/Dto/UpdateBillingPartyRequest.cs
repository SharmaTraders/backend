namespace Dto;

public record UpdateBillingPartyRequest(
    string Name,
    string Address,
    string? PhoneNumber,
    string? Email,
    string? VatNumber) {
}