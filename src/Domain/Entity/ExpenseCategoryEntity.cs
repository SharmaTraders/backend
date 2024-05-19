using Domain.common;

namespace Domain.Entity;

public class ExpenseCategoryEntity :  IEntity<string> {
    private string _name;
    public required string Name {
        get => _name;
        set {
            ValidateName(value);
            _name = value.Trim();
        }
    }

    private static void ValidateName(string name) {
        if (string.IsNullOrWhiteSpace(name)) {
            throw new DomainValidationException("Name", ErrorCode.BadRequest,
                ErrorMessages.ExpenseCategoryNameRequired);
        }

        if (name is not {Length: >= 2 and <= 1000}) {
            throw new DomainValidationException("Name", ErrorCode.BadRequest,
                ErrorMessages.ExpenseCategoryNameBetween2And100);
        }
    }
    
}