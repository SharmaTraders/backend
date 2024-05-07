using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class AdminEntity : IEntity<Guid> {
    [Key] public required Guid Id { get; init; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}