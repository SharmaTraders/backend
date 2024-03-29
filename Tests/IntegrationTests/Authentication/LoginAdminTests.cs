﻿using System.Net;
using Domain.utils;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegrationTests.Authentication;

public class LoginAdminTests {
    private readonly WebApp _application = new();

    [Fact]
    public async Task LoginAdmin_WithValidEmailAndValidPassword_ReturnsJwtTokenAndOkResponse() {
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
        HttpResponseMessage response = await client.SendAsync(request);

        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert that the jwtToken is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(responseContent);

        Assert.NotNull(loginRequestDto);
        Assert.False(string.IsNullOrEmpty(loginResponseDto!.JwtToken));
    }

    [Fact]
    public async Task LoginAdmin_WithValidEmailAnd_InValidPassword_ReturnsBadRequestResponse_WithCorrectErrorMessage() {
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
        HttpResponseMessage response = await client.SendAsync(request);

        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Assert that the correct error message is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.IncorrectPassword, problemDetails.Detail);
    }

    [Fact]
    public async Task
        LoginAdmin_WithInValidEmailAnd_InValidPassword_ReturnsBadRequestResponse_WithCorrectErrorMessage() {
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
        HttpResponseMessage response = await client.SendAsync(request);

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