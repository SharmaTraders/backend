using System.Net;
using Domain.Entity;
using Dto;
using IntegrationTests.FakeDbSetup;
using Newtonsoft.Json;

namespace IntegrationTests.TestFactory;

internal static class UserFactory {
    internal static AdminDto GetValidAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "admin@admin.com");

    internal static LoginRequest GetValidLoginRequest() =>
        new LoginRequest("admin@admin.com", "somePassword1234");

    internal static AdminEntity GetValidAdminEntity() =>
        new AdminEntity() {
            Id = Guid.NewGuid(),
            Email = "admin@admin.com",
            Password = HashPassword("somePassword1234")
        };


    internal static AdminEntity GetAdminEntity(string email) =>
        new AdminEntity() {
            Id = Guid.NewGuid(),
            Email = email,
            Password = HashPassword("somePassword1234")
        };


    internal static AdminDto GetInvalidAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "badEmail");

    internal static LoginRequest GetInvalidEmailRequest() =>
        new LoginRequest("badEmail", "goodPassword1");

   internal static LoginRequest GetInvalidPasswordLessThan5CharsRequest() =>
        new LoginRequest("goodEmail@email.com", "bad");


    internal static LoginRequest GetInvalidPasswordLessThanNoLettersAndNumbersAdmin() =>
        new LoginRequest("goodemail@email.com", "badButBigger");



    internal static async Task<string> SetupLoggedInAdmin(WebApp app) {
        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdminEntity();
        await SeedData.SeedAdmin(app, adminUser);

        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login/admin");
        var loginRequestDto = new LoginRequest(adminUser.Email, "somePassword1234");
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), System.Text.Encoding.UTF8,
            "application/json");

        HttpClient client = app.CreateClient();

        HttpResponseMessage response = await client.SendAsync(request);
        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert that the jwtToken is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponseDto = JsonConvert.DeserializeObject<LoginResponse>(responseContent);

        Assert.NotNull(loginRequestDto);
        Assert.False(string.IsNullOrEmpty(loginResponseDto!.JwtToken));

        return loginResponseDto.JwtToken;
    }

    private static string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}