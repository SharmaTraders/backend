using System.ComponentModel.DataAnnotations;
using Domain.utils;

namespace Domain.Entity;

public class ItemEntity : IEntity<Guid> {

    private string _name;
    [Key] public  Guid Id { get; set; }
    
    [Required]
    public required string Name {
        get => _name;
        set {
            ValidateName(value);
            _name = value;
        }
    }


    private static void ValidateName(string value) {
        if (string.IsNullOrEmpty(value)) {
            throw new DomainValidationException("ItemName", ErrorCode.BadRequest, ErrorMessages.ItemNameIsRequired);
        }

        value = value.Trim();
        if (value.Length is < 3 or > 20) {
            throw new DomainValidationException("ItemName", ErrorCode.BadRequest, ErrorMessages.ItemNameLength);
        }
    }
}