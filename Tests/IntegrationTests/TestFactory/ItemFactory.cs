using Dto;

namespace IntegrationTests.TestFactory;

internal class ItemFactory {
    internal static CreateItemRequest GetValidItemDto() {
        return new CreateItemRequest("Gadda");
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
            new CreateItemRequest("Gadda"),
            new CreateItemRequest("Thaal")
        ];
    }
}