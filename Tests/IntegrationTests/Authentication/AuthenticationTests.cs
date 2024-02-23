using System.Net;
using Domain.utils;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;

namespace IntegrationTests.Authentication;

public class AuthenticationTests {
  


    [Fact]
    public async Task RegisterAdminWith_NoToken_Fails() {
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Generate a jwt for an admin

        // When no token is set
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetValidAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

        // Act
        var response = await client.SendAsync(request );

        // Assert that the result is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task RegisterAdminWith_AdminToken_Succeeds() {
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Generate a jwt for an admin
        string validAdminToken = UserFactory.GetValidAdminJwtToken();

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetValidAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

        // Act
        var response = await client.SendAsync(request );

        // Assert  that the register succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RegisterAdminWith_EmployeeToken_Fails() {
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Generate a jwt for an admin
        string validEmployeeToken = UserFactory.GetValidEmployeeJwtToken();

        // Arrange a request with a valid Employee token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validEmployeeToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetValidAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

        // Act
        var response = await client.SendAsync(request );

        // Assert that the response is Forbidden
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RegisterAdminWith_AdminToken_InvalidEmail_Fails() {
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Generate a jwt for an admin
        string validAdminToken = UserFactory.GetValidAdminJwtToken();

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetInvalidEmailAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

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
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Generate a jwt for an admin
        string validAdminToken = UserFactory.GetValidAdminJwtToken();

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetInvalidPasswordLessThan5CharsAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

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
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Generate a jwt for an admin
        string validAdminToken = UserFactory.GetValidAdminJwtToken();

        // Arrange a request with a valid admin token
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register/admin");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(UserFactory.GetInvalidPasswordLessThanNoLettersAndNumbersAdmin()), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

        // Act
        HttpResponseMessage response = await client.SendAsync(request );

        // Assert  that the register fails with the correct error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(errorContent);
        Assert.Equal(ErrorMessages.PasswordMustContainLetterAndNumber, errorContent);
    }


    [Fact]
    public async Task LoginAdminWith_ValidEmailAndValidPassword_ReturnsJwtTokenAndOkResponse() {
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdmin();
        await SeedData.SeedAdmin(application, adminUser);


        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login/admin");
        var loginRequestDto = new LoginRequestDto(adminUser.Email, adminUser.Password);
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

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
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdmin();
        await SeedData.SeedAdmin(application, adminUser);


        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login/admin");
        var loginRequestDto = new LoginRequestDto(adminUser.Email, "someIncorrectPassword1");
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

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
        // Arrange a custom web apllicaiton/ this is based on our program class
        var application = new CustomWebApplicationFactory();

        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdmin();
        await SeedData.SeedAdmin(application, adminUser);


        // Arrange a request with an existing admin
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login/admin");
        var loginRequestDto = new LoginRequestDto("incorrect@email.com", "someIncorrectPassword1");
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = application.CreateClient();

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