using System.Net;
using Dto;
using IntegrationTests.FakeDbSetup;
using Newtonsoft.Json;

namespace IntegrationTests.TestFactory;

internal static class UserFactory {
    internal static AdminDto GetValidAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "admin@admin.com", "somePassword1234");

    internal static AdminDto GetInvalidAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "badEmail", "badPassword");

    internal static AdminDto GetInvalidEmailAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "badEmail", "goodPassword1");

    internal static AdminDto GetInvalidPasswordLessThan5CharsAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "goodemail@email.com", "bad");

    internal static AdminDto GetInvalidPasswordLessThanNoLettersAndNumbersAdmin() =>
        new AdminDto(Guid.NewGuid().ToString(), "goodemail@email.com", "badButBigger");

    internal static EmployeeDto GetValidEmployee() =>
        new EmployeeDto(Guid.NewGuid().ToString(), "employee@employee.com", "somePassword1234", "test user", "asdfsf",
            "12345678", "Active");

    internal static EmployeeDto GetInvalidEmployee() =>
        new EmployeeDto(Guid.NewGuid().ToString(), "badEmail", "badPassword", "test user", "asdfsf",
            "12345678", "Active");

    internal static async Task<string> SetupLoggedInAdmin(WebApp app) {
        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdmin();
        await SeedData.SeedAdmin(app, adminUser);

        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login/admin");
        var loginRequestDto = new LoginRequestDto(adminUser.Email, adminUser.Password);
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), System.Text.Encoding.UTF8,
            "application/json");

        HttpClient client = app.CreateClient();

        HttpResponseMessage response = await client.SendAsync(request );
        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert that the jwtToken is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(responseContent);

        Assert.NotNull(loginRequestDto);
        Assert.False(string.IsNullOrEmpty(loginResponseDto!.JwtToken));

        return loginResponseDto.JwtToken;

    }

}