namespace UnitTests.Factory
{
    public static class ExpenseCategoryFactory
    {
        public static IEnumerable<object[]> GetValidExpenseCategoryNames()
        {
            return new List<object[]>
            {
                new object[] { "Office Supplies" },
                new object[] { "Travel Expenses" },
                new object[] { "Utilities" },
                new object[] { "a".PadRight(1000, 'a') }
            };
        }

        public static IEnumerable<object[]> GetInvalidExpenseCategoryNames()
        {
            return new List<object[]>
            {
                new object[] { "" }, // Empty string
                new object[] { " " }, // Whitespace
                new object[] { "a" }, // Too short
                new object[] { "a".PadRight(1001, 'a') } // Too long
            };
        }
    }
}