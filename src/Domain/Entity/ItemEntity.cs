using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class ItemEntity : IEntity<Guid> {
    [Key] public required Guid Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
}