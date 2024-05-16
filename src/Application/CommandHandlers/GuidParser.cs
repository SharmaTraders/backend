using Tools;

namespace Application.CommandHandlers;

public static class GuidParser {

    public static Guid ParseGuid(string id, string type) {
        if (!Guid.TryParse(id, out Guid parsedGuid)) {
            throw new DomainValidationException(type, ErrorCode.BadRequest, ErrorMessages.IdInvalid(id));
        }
        return parsedGuid;
    }
    
}