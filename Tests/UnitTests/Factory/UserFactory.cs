using Domain.Entity;

namespace UnitTests.Factory;

internal static class UserFactory {
    public static IEnumerable<object[]> GetValidEmails() {
        return new List<object[]>() {
            new object[] {"310628@via.dk"},
            new object[] {"ALHE@via.dk"},
            new object[] {"TRMO@via.dk"},
            new object[] {"sachinbaral@hotmail.dk"},
            new object[] {"himalSharma123@yahoo.dk"},
        };
    }

    public static IEnumerable<object[]> GetInValidEmails() {
        return new List<object[]>() {
            new object[] {"@via.dk"},
            new object[] {""},
            new object[] {"   "},
            new object[] {"Hello"},
            new object[] {"Hello@"},
            new object[] {"Hello@gmail"},
            new object[] {"Hello.com"},
        };
    }

    public static IEnumerable<object[]> GetValidPasswords() {
        return new List<object[]>() {
            new object[] {"123abc"},
            new object[] {"someVeryLongPassword123"},
            new object[] {"someVeryLongPasswordWithNumber1andSpecialChar!"},
        };
    }

    public static IEnumerable<object[]> GetInvalidPasswords() {
        return new List<object[]>() {
            new object[] {""},
            new object[] {"   "},
            new object[] {"       "},
            new object[] {"LongWithoutNumber"},
            new object[] {"short"},
            new object[] {"a"},
            new object[] {"123456789"},
        };
    }

    public static AdminEntity GetValidAdminEntity() {
        return new AdminEntity() {
            Id = Guid.NewGuid(),
            Email = "valid@valid.com",
            Password = HashPassword("somePassword1234")
        };
    }

    private static string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}