using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class AdminEntity {
    [Key] public Guid Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}