using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ItemEntity {
    [Key] 
    public required string Name { get; set; }
}