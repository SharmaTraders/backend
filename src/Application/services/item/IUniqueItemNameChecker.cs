using System.Runtime.InteropServices;

namespace Application.services.item;

public interface IUniqueItemNameChecker {

    Task<bool> IsUniqueAsync(string name, [Optional] Guid idToExclude);
    
}