using System.Net;
using Domain.utils;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;

namespace IntegrationTests.Authentication;

public class AuthenticationTests {

    private readonly WebApp _application = new WebApp();


    [Fact]
    public async Task RegisterAdminWith_NoToken_Fails() {

        // When no token is set
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetValidAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request );

        // Assert that the result is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task RegisterAdminWith_AdminToken_Succeeds() {

        // Setup logged in admin
        string validAdminToken =await UserFactory.SetupLoggedInAdmin(_application);

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        RegisterAdminRequestDto requestDto = new RegisterAdminRequestDto("hello@gmail.com", "somePassword12");
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request );

        // Assert  that the register succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

 

    [Fact]
    public async Task RegisterAdminWith_AdminToken_InvalidEmail_Fails() {


        // Setup a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetInvalidEmailAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request );

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(errorContent);
        Assert.Equal(ErrorMessages.InvalidEmailFormat, errorContent);
    }

    [Fact]
    public async Task RegisterAdminWith_AdminToken_InvalidPassword_LessThan5Chars_Fails() {

        // Setup a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetInvalidPasswordLessThan5CharsAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request );

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(errorContent);
        Assert.Equal(ErrorMessages.PasswordBiggerThan5Characters, errorContent);
    }

    [Fact]
    public async Task RegisterAdminWith_AdminToken_InvalidPassword_NotLetterAndNumber_Fails() {
        // Setup a logged in admin
        string validAdminToken =await UserFactory.SetupLoggedInAdmin(_application);

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetInvalidPasswordLessThanNoLettersAndNumbersAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request );

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(errorContent);
        Assert.Equal(ErrorMessages.PasswordMustContainLetterAndNumber, errorContent);
    }

    [Fact]
    public async Task RegisterAdmin_WithAdminToken_WhenAdminAlreadyExists_Fails() {
        // Setup a logged in admin
        string validAdminToken =await UserFactory.SetupLoggedInAdmin(_application);
        const string existingEmail = "sharmatraders@gmail.com";

        AdminDto existingAdmin = new AdminDto(Guid.NewGuid().ToString(), existingEmail, "somePassword12");

        // Arrange an already existing admin in db
        await SeedData.SeedAdmin(_application, existingAdmin);

        // When registering with the existing email
        RegisterAdminRequestDto registerAdminRequest = new RegisterAdminRequestDto(existingEmail, "someOtherPassword12");
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(registerAdminRequest), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request);

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(errorContent);
        Assert.Equal(ErrorMessages.AdminWithEmailAlreadyExists, errorContent);




    }

    
    [Fact]
    public async Task RegisterAdminWith_EmployeeToken_Fails() {

        // Todo this test needs to be done after implementing the employee login.
    }

    [Fact]
    public async Task LoginAdminWith_ValidEmailAndValidPassword_ReturnsJwtTokenAndOkResponse() {


        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdmin();
        await SeedData.SeedAdmin(_application, adminUser);


        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login/admin");
        var loginRequestDto = new LoginRequestDto(adminUser.Email, adminUser.Password);
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request );

        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert that the jwtToken is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(responseContent);

        Assert.NotNull(loginRequestDto);
        Assert.False(string.IsNullOrEmpty(loginResponseDto!.JwtToken));

    }

    [Fact]
    public async Task LoginAdminWith_ValidEmailAnd_InValidPassword_ReturnsBadRequestResponse_WithCorrectErrorMessage() {

        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdmin();
        await SeedData.SeedAdmin(_application, adminUser);


        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login/admin");
        var loginRequestDto = new LoginRequestDto(adminUser.Email, "someIncorrectPassword1");
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request );

        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Assert that the correct error message is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        Assert.Equal(ErrorMessages.IncorrectPassword, responseContent);
    }

    [Fact]
    public async Task LoginAdminWith_InValidEmailAnd_InValidPassword_ReturnsBadRequestResponse_WithCorrectErrorMessage() {


        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdmin();
        await SeedData.SeedAdmin(_application, adminUser);


        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login/admin");
        var loginRequestDto = new LoginRequestDto("incorrect@email.com", "someIncorrectPassword1");
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request );

        // Assert  that the login fails
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        // Assert that the correct error message is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        Assert.Equal(ErrorMessages.EmailDoesntExist, responseContent);
    }





    
}