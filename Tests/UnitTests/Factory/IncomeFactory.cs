namespace UnitTests.Factory;

public class IncomeFactory {
    public static IEnumerable<object[]> GetValidDates() {
        return new List<object[]>() {
            new object[] {new DateOnly()},
            new object[] {DateOnly.FromDateTime(DateTime.Now.AddDays(-1))},
            new object[] {DateOnly.FromDateTime(DateTime.Now.AddDays(-10))},
            new object[] {DateOnly.FromDateTime(DateTime.Now.AddYears(-10))},
        };
    }

    public static IEnumerable<object[]> GetInvalidDates() {
        return new List<object[]>() {
            new object[] {DateOnly.FromDateTime(DateTime.Now.AddDays(1))},
            new object[] {DateOnly.FromDateTime(DateTime.Now.AddDays(10))},
            new object[] {DateOnly.FromDateTime(DateTime.Now.AddYears(10))},
        };
    }


    public static IEnumerable<object[]> GetValidAmounts() {
        return new List<object[]>() {
            new object[] {1},
            new object[] {125},
            new object[] {100.23},
            new object[] {15000.21},
        };
    }

    public static IEnumerable<object[]> GetInvalidAmounts() {
        return new List<object[]>() {
            new object[] {-1},
            new object[] {0},
            new object[] {-100},
            new object[] {100.222},
        };
    }
}