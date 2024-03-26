using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class BillingPartyEntity {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Address { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    [Required, DefaultValue(0)]
    public double Balance { get; set; }

    public string? VatNumber { get; set; }
}