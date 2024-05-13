using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Endpoints.command.authentication;
using Xunit.Abstractions;

namespace IntegrationTests.Authentication;

public class RegisterAdminTests : BaseIntegrationTest {
    private readonly ITestOutputHelper _testOutputHelper;

    public RegisterAdminTests(IntegrationTestsWebAppFactory application, ITestOutputHelper testOutputHelper) : base(application) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task RegisterAdmin_WithNoToken_Fails() {
        // When no token is set
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register/admin");

        RegisterAdminRequest registerAdminRequest = new RegisterAdminRequest() {
            RequestBody = new RegisterAdminRequest.Body("hello@gmail.com", "somePassword12")
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(registerAdminRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");


        // Act
        var response = await Client.SendAsync(request);

        // Assert that the result is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task RegisterAdmin_WithAdminToken_Succeeds() {
        // Setup logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        int adminCount = await WriteDbContext.Admins.CountAsync();
        _testOutputHelper.WriteLine(adminCount.ToString());

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        RegisterAdminRequest registerAdminRequest = new RegisterAdminRequest() {
            RequestBody = new RegisterAdminRequest.Body("hello@gmail.com", "somePassword12")
        };        request.Content = new StringContent(JsonConvert.SerializeObject(registerAdminRequest.RequestBody), System.Text.Encoding.UTF8,
            "application/json");


        // Act
        var response = await Client.SendAsync(request);

        // Assert  that the register succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task RegisterAdmin_WithAdminToken_InvalidEmail_Fails() {
        // Setup a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        RegisterAdminRequest registerAdminRequest = new RegisterAdminRequest() {
            RequestBody = new RegisterAdminRequest.Body("hellogmail.com", "somePassword12")
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(registerAdminRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");


        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

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
        string validAdminToken = await SetupLoggedInAdmin();

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        RegisterAdminRequest registerAdminRequest = new RegisterAdminRequest() {
            RequestBody = new RegisterAdminRequest.Body("hello@gmail.com", "some")
        };
        request.Content = new StringContent(
            JsonConvert.SerializeObject(registerAdminRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");


        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.AdminPasswordBiggerThan5Characters, problemDetails.Detail);
    }

    [Fact]
    public async Task RegisterAdmin_WithAdminToken_InvalidPassword_NotLetterAndNumber_Fails() {
        // Setup a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        RegisterAdminRequest registerAdminRequest = new RegisterAdminRequest() {
            RequestBody = new RegisterAdminRequest.Body("hello@gmail.com", "badButBigger")
        };
        request.Content = new StringContent(
            JsonConvert.SerializeObject(registerAdminRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);
        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.AdminPasswordMustContainLetterAndNumber, problemDetails.Detail);
    }

    [Fact]
    public async Task Register_AdminWithAdminToken_WhenAdminAlreadyExists_Fails() {
        // Setup a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();
        const string existingEmail = "sharmatraders@gmail.com";

        AdminEntity existingAdmin = UserFactory.GetAdminEntity(existingEmail);

        // Arrange an already existing admin in db
        await SeedData.SeedAdmin(WriteDbContext, existingAdmin);

        // When registering with the existing email
        RegisterAdminRequest registerAdminRequest = new RegisterAdminRequest() {
            RequestBody = new RegisterAdminRequest.Body(existingEmail, "somePassword12")
        };
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(registerAdminRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.AdminEmailAlreadyExists, problemDetails.Detail);
    }


    [Fact]
    public async Task RegisterAdmin_WithEmployeeToken_Fails() {
        // Todo this test needs to be done after implementing the employee login.
    }
}