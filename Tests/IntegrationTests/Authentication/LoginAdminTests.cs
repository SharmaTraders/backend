using System.Net;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Endpoints.command.authentication;
using Xunit.Abstractions;

namespace IntegrationTests.Authentication;

public class LoginAdminTests : BaseIntegrationTest {

    public LoginAdminTests(IntegrationTestsWebAppFactory application) : base(application) {
    }


    [Fact]
    public async Task LoginAdmin_WithValidEmailAndValidPassword_ReturnsJwtTokenAndOkResponse() {
        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdminEntity();
        await SeedData.SeedAdmin(WriteDbContext, adminUser);

        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login/admin");
        var loginRequestDto = UserFactory.GetValidLoginRequest();
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto.RequestBody), System.Text.Encoding.UTF8,
            "application/json");


        // Act
        HttpResponseMessage response = await Client.SendAsync(request);



        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert that the jwtToken is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponseDto = JsonConvert.DeserializeObject<LoginAdminResponse>(responseContent);

        Assert.NotNull(loginResponseDto);
        Assert.False(string.IsNullOrEmpty(loginResponseDto!.JwtToken));
    }

    [Fact]
    public async Task LoginAdmin_WithValidEmailAnd_InValidPassword_ReturnsBadRequestResponse_WithCorrectErrorMessage() {
        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdminEntity();
        await SeedData.SeedAdmin(WriteDbContext, adminUser);


        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login/admin");
        var loginRequestDto = new LoginAdminRequest() {
            RequestBody = new LoginAdminRequest.Body(adminUser.Email, "someIncorrectPassword1")
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto.RequestBody), System.Text.Encoding.UTF8,
            "application/json");


        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Assert that the correct error message is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.PasswordIncorrect, problemDetails.Detail);
    }

    [Fact]
    public async Task
        LoginAdmin_WithInValidEmailAnd_InValidPassword_ReturnsBadRequestResponse_WithCorrectErrorMessage() {
        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdminEntity();
        await SeedData.SeedAdmin(WriteDbContext, adminUser);


        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login/admin");
        var loginRequestDto = new LoginAdminRequest() {
            RequestBody = new LoginAdminRequest.Body("incorrect@email.com", "someIncorrectPassword1")
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto.RequestBody), System.Text.Encoding.UTF8,
            "application/json");


        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert  that the login fails
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        // Assert that the correct error message is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.EmailDoesntExist, problemDetails.Detail);
    }
}