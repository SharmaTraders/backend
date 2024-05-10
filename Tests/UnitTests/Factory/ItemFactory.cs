namespace UnitTests.Factory;

internal static class ItemFactory {
    
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

    public static IEnumerable<object[]> GetValidOpeningStocks() {
        return new List<object[]>() {
            new object[] {0},
            new object[] {1},
            new object[] {100},
            new object[] {50.25},
        };
    } 
    public static IEnumerable<object[]> GetInValidOpeningStocks() {
        return new List<object[]>() {
            new object[] {-1},
            new object[] {-100},
            new object[] {-10.54},
        };
    }
    public static IEnumerable<object[]> GetValidValuePerKilo() {
        return new List<object[]>() {
            new object[] {0},
            new object[] {1},
            new object[] {100},
            new object[] {50.25},
        };
    } 
    public static IEnumerable<object[]> GetInValidValuePerKilo() {
        return new List<object[]>() {
            new object[] {-1},
            new object[] {-100},
            new object[] {-10.54},
        };
    }
}