using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class AdminEntity {
    [Key] public required Guid Id { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}