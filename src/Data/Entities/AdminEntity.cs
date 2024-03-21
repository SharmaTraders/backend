using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class AdminEntity {
    [Key] public required Guid Id { get; init; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}