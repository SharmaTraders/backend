namespace UnitTests.Factory;

public static class RemarksFactory {
    public static IEnumerable<object[]> GetValidRemarks() {
        return new List<object[]>() {
            new object[] {null},
            new object[] {""},
            new object[] {"a".PadRight(20, 'a')},
            new object[] {"a".PadRight(500, 'a')}
        };
    }


    public static IEnumerable<object[]> GetInvalidRemarks() {
        return new List<object[]>() {
            new object[] {"a".PadRight(501, 'a')},
            new object[] {"a".PadRight(1000, 'a')}
        };
    }
}