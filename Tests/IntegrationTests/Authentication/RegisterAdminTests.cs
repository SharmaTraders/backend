using System.Net;
using Domain.Entity;
using Domain.utils;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegrationTests.Authentication;

[Collection("Sequential")]
public class RegisterAdminTests {
    private readonly WebApp _application = new();

    [Fact]
    public async Task RegisterAdmin_WithNoToken_Fails() {
        // When no token is set
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetValidAdmin()),
            System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert that the result is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task RegisterAdmin_WithAdminToken_Succeeds() {
        // Setup logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        RegisterAdminRequest registerRequest = new RegisterAdminRequest("hello@gmail.com", "somePassword12");
        request.Content = new StringContent(JsonConvert.SerializeObject(registerRequest), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert  that the register succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task RegisterAdmin_WithAdminToken_InvalidEmail_Fails() {
        // Setup a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetInvalidEmailRequest()),
            System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request);

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.EmailInvalidFormat, problemDetails.Detail);
    }

    [Fact]
    public async Task RegisterAdmin_WithAdminToken_InvalidPassword_LessThan5Chars_Fails() {
        // Setup a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(
            JsonConvert.SerializeObject(UserFactory.GetInvalidPasswordLessThan5CharsRequest()), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request);

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.PasswordBiggerThan5Characters, problemDetails.Detail);
    }

    [Fact]
    public async Task RegisterAdmin_WithAdminToken_InvalidPassword_NotLetterAndNumber_Fails() {
        // Setup a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(
            JsonConvert.SerializeObject(UserFactory.GetInvalidPasswordLessThanNoLettersAndNumbersAdmin()),
            System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request);

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.PasswordMustContainLetterAndNumber, problemDetails.Detail);    }

    [Fact]
    public async Task Register_AdminWithAdminToken_WhenAdminAlreadyExists_Fails() {
        // Setup a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);
        const string existingEmail = "sharmatraders@gmail.com";

        AdminEntity existingAdmin = UserFactory.GetAdminEntity(existingEmail);

        // Arrange an already existing admin in db
        await SeedData.SeedAdmin(_application, existingAdmin);

        // When registering with the existing email
        RegisterAdminRequest registerAdminRequest =
            new RegisterAdminRequest(existingEmail, "someOtherPassword12");
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(registerAdminRequest),
            System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request);

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.EmailAlreadyExists, problemDetails.Detail);
    }


    [Fact]
    public async Task RegisterAdmin_WithEmployeeToken_Fails() {
        // Todo this test needs to be done after implementing the employee login.
    }
}