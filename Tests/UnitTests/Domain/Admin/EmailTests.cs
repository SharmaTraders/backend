using Domain.Entity;
using UnitTests.Factory;

namespace UnitTests.Domain.Admin;

public class EmailTests {
    
    [Theory]
    [MemberData(nameof(UserFactory.GetValidEmails), MemberType = typeof(UserFactory))]
    public void AdminWith_ValidEmail_CanBeCreated(string email) {
        // Act
        var userEntity = new AdminEntity() {
            Email = email,
            Password = "ValidPassword12"
        };
        // No exception is thrown
    }

    [Theory]
    [MemberData(nameof(UserFactory.GetInValidEmails), MemberType = typeof(UserFactory))]

    public void AdminWith_InvalidEmail_ThrowsException(string email) {
        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => new AdminEntity() {
            Email = email,
            Password = "ValidPassword12"
        });
        Assert.Equal("email", exception.Type.ToLower());
    }

}