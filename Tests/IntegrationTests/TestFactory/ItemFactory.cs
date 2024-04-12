using Dto;

namespace IntegrationTests.TestFactory;

internal class ItemFactory {

    internal static ItemDto GetValidItemDto() {
        return new ItemDto("Gadda");
    }

    public static List<ItemDto> GetValidItemsList() {
        return new List<ItemDto>() {
            new ItemDto("Gadda"),
            new ItemDto("Gadda2"),
            new ItemDto("Gadda3"),
            new ItemDto("Gadda4"),
            new ItemDto("Gadda5"),
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

}