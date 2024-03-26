namespace Dto;

public record BillingPartyDto (string Id,
    string Name,
    string Address,
    string? Email,
    string? PhoneNumber,
    double? Balance,
    string? VatNumber);