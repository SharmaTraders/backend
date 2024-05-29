namespace UnitTests.Factory
{
    public static class EmployeeFactory
    {
        public static IEnumerable<object[]> GetValidEmployeeNames()
        {
            return new List<object[]>
            {
                new object[] { "John Doe" },
                new object[] { "Jane Smith" },
                new object[] { "HHH" },
                new object[] { "a".PadRight(30, 'a') },
                new object[] { "a".PadRight(29, 'a') }
            };
        }

        public static IEnumerable<object[]> GetInvalidEmployeeNames()
        {
            return new List<object[]>
            {
                new object[] { "" }, // Empty string
                new object[] { "A" }, // Too short
                new object[] { new string('A', 51) } // Too long
            };
        }

        public static IEnumerable<object[]> GetValidEmployeeAddresses()
        {
            return new List<object[]>
            {
                new object[] { "123 Main St" },
                new object[] { "456 Elm St" },
                new object[] { "a".PadRight(60, 'a') },
                new object[] { "a".PadRight(59, 'a') }
            };
        }

        public static IEnumerable<object[]> GetInvalidEmployeeAddresses()
        {
            return new List<object[]>
            {
                new object[] { "" }, // Empty string
                new object[] { "A" }, // Too short
                new object[] { new string('A', 61) } // Too long
            };
        }

        public static IEnumerable<object[]> GetValidEmployeeEmails()
        {
            return new List<object[]>
            {
                new object[] { "test@example.com" },
                new object[] { "user@domain.com" },
                new object[] { null },
                new object[] { "" }
            };
        }

        public static IEnumerable<object[]> GetInvalidEmployeeEmails()
        {
            return new List<object[]>
            {
                new object[] { "invalid-email" }, // No @ symbol
                new object[] { "user@domain" }, // No domain suffix
                new object[] { "Hello@" },
                new object[] { "Hello@gmail" },
                new object[] { "Hello.com" }
            };
        }

        public static IEnumerable<object[]> GetValidEmployeePhoneNumbers()
        {
            return new List<object[]>
            {
                new object[] { "1234567890" },
                new object[] { "0987654321" },
                new object[] { null },
                new object[] { "" }
            };
        }

        public static IEnumerable<object[]> GetInvalidEmployeePhoneNumbers()
        {
            return new List<object[]>
            {
                new object[] { "123" }, // Too short
                new object[] { "abcdefghij" }, // Non-numeric
                new object[] { "123456789" },
                new object[] { "98460380502500" }
            };
        }

        public static IEnumerable<object[]> GetValidNormalDailyWorkingMinutes()
        {
            return new List<object[]>
            {
                new object[] { 480 }, // 8 hours
                new object[] { 600 } // 10 hours
            };
        }

        public static IEnumerable<object[]> GetInvalidNormalDailyWorkingMinutes()
        {
            return new List<object[]>
            {
                new object[] { -1 }, // Negative minutes
                new object[] { 1500 } // Exceeds 24 hours
            };
        }
    }
}