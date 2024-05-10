
using Domain.Entity;

using WebApi.Endpoints.command.authentication;

namespace IntegrationTests.TestFactory;

internal static class UserFactory {

    internal static LoginAdminRequest GetValidLoginRequest() =>
        new () {
            RequestBody = new LoginAdminRequest.Body("admin@admin.com", "somePassword1234")
        };

    internal static AdminEntity GetValidAdminEntity() =>
        new AdminEntity() {
            Id = Guid.NewGuid(),
            Email = "admin@admin.com",
            Password = "somePassword1234"
        };


    internal static AdminEntity GetAdminEntity(string email) =>
        new AdminEntity() {
            Id = Guid.NewGuid(),
            Email = email,
            Password = "somePassword1234"
        };



   

}