namespace UnitTests.Factory;

public static class ExpenseFactory
{
    public static IEnumerable<object[]> GetValidDates()
    {
        return new List<object[]>
        {
            new object[] { DateOnly.FromDateTime(DateTime.Now) },
            new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) },
            new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(-10)) },
            new object[] { DateOnly.FromDateTime(DateTime.Now.AddYears(-10)) }
        };
    }

    public static IEnumerable<object[]> GetInvalidDates()
    {
        return new List<object[]>
        {
            new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(1)) },
            new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(10)) },
            new object[] { DateOnly.FromDateTime(DateTime.Now.AddYears(10)) }
        };
    }

    public static IEnumerable<object[]> GetValidAmounts()
    {
        return new List<object[]>
        {
            new object[] { 1 },
            new object[] { 125 },
            new object[] { 100.23 },
            new object[] { 15000.21 }
        };
    }

    public static IEnumerable<object[]> GetInvalidAmounts()
    {
        return new List<object[]>
        {
            new object[] { -1 },
            new object[] { 0 },
            new object[] { -100 },
            new object[] { 100.222 }
        };
    }

    public static IEnumerable<object[]> GetValidRemarks()
    {
        return new List<object[]>
        {
            new object[] { "Valid remarks" },
            new object[] { "Another valid remark" },
            new object[] { "" },
            new object[] { null }
        };
    }

    public static IEnumerable<object[]> GetInvalidRemarks()
    {
        return new List<object[]>
        {
            new object[] { new string('a', 1001) }
        };
    }
}