
using WebApi.Endpoints.command.item;

namespace IntegrationTests.TestFactory;

internal class ItemFactory {
    internal static CreateItemRequest GetValidCreateItemRequest() {
        return new CreateItemRequest() {
            RequestBody = new CreateItemRequest.Body("Gadda", 10, 100)
        };
    }


    public static IEnumerable<object[]> GetValidItemNames() {
        return new List<object[]>() {
            new object[] {"thr"},
            new object[] {"a".PadRight(20, 'a')},
            new object[] {"GATTA"},
            new object[] {"gatta"},
            new object[] {"hamiNepaliHamroNepal"},
            new object[] {"someitem name"},
            new object[] {"someitem name12"},
        };
    }

    public static IEnumerable<object[]> GetInValidItemNames() {
        return new List<object[]>() {
            new object[] {""},
            new object[] {"a".PadLeft(21, 'a')},
            new object[] {"a".PadLeft(50, 'a')},
            new object[] {"hami Nepali Hamro Nepal"},
        };
    }

    public static List<CreateItemRequest> GetCreateItemRequestsList() {
        return [
            GetValidCreateItemRequest(),
            new CreateItemRequest() {
                RequestBody = new CreateItemRequest.Body("Thaal", 100, 60)
            }
        ];
    }
}