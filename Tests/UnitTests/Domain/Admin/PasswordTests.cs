using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Admin;

public class PasswordTests {

    [Theory]
    [MemberData(nameof(UserFactory.GetValidPasswords), MemberType = typeof(UserFactory))]
    public void AdminWith_ValidPasswords_CanBeCreated(string password) {
        var admin = new AdminEntity() {
            Email = "valid@valid.com",
            Password = password
        };

        // No exception is thrown

    }

    [Theory]
    [MemberData(nameof(UserFactory.GetInvalidPasswords), MemberType = typeof(UserFactory))]
    public void AdminWith_InvalidPassword_CannotBeCreated(string password) {
        var exception = Assert.Throws<DomainValidationException>( () =>new AdminEntity() {
            Email = "valid@valid.com",
            Password = password
        });

        Assert.Equal("password", exception.Type.ToLower());
    }

    
}