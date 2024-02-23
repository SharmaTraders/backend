using System.Security.Claims;
using Domain.auth;
using Domain.utils;
using Dto;
using Dto.tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using WebApi.Controllers;

namespace UnitTests.Controllers;

public class AuthControllerTests {


    // We dont make setup for the tests to ensure isolation. This is also why we use xUnit and not MSTest or NUnit
    [Fact]
    public async Task AdminLogin_ValidCredentials_ReturnsOk() {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var authDomainMock = new Mock<IAuthenticationDomain>();

        AuthController authController = new AuthController(configurationMock.Object, authDomainMock.Object);

        var loginRequestDto = new LoginRequestDto("test@example.dk", "password12");
        var userDto = new AdminDto("id", "test@example.dk", "password12");
        configurationMock.Setup(x => x["JWT:SecretKey"]).Returns("your_secret_key_that_is_not_too_short_and_is_too_long_2-3-3-4-a-sdasdasdasdasdasdas");
        configurationMock.Setup(x => x["JWT:Issuer"]).Returns("your_issuer");
        configurationMock.Setup(x => x["JWT:Audience"]).Returns("your_audience");
        configurationMock.Setup(x => x["JWT:Subject"]).Returns("your_subject");   
        
        authDomainMock.Setup(a => a.ValidateAdmin(loginRequestDto)).ReturnsAsync((UserDto)userDto);

        // Act
        ActionResult<LoginResponseDto> rootResult = await authController.AdminLogin(loginRequestDto);

        
        // Assert that Ok response is returned
        Assert.IsType<OkObjectResult>(rootResult.Result);

        
        var result = rootResult?.Result as ObjectResult;
        Assert.NotNull(result);

        // Assert that the response contains a token
        var responseDto = result.Value as LoginResponseDto;
        Assert.NotNull(responseDto);
        Assert.NotEmpty(responseDto.JwtToken);
    }



    [Fact]
    public async Task AdminLogin_InValidCredentials_ReturnsNotOk() {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var authDomainMock = new Mock<IAuthenticationDomain>();

        AuthController authController = new AuthController(configurationMock.Object, authDomainMock.Object);

        var loginRequestDto = new LoginRequestDto("test@example.dk", "password12");
        configurationMock.Setup(x => x["JWT:SecretKey"]).Returns("your_secret_key_that_is_not_too_short_and_is_too_long_2-3-3-4-a-sdasdasdasdasdasdas");
        configurationMock.Setup(x => x["JWT:Issuer"]).Returns("your_issuer");
        configurationMock.Setup(x => x["JWT:Audience"]).Returns("your_audience");
        configurationMock.Setup(x => x["JWT:Subject"]).Returns("your_subject");   
        
        authDomainMock.Setup(a => a.ValidateAdmin(loginRequestDto)).ThrowsAsync(new ExceptionWithErrorCode(ErrorCode.BadRequest, ErrorMessages.PasswordMustContainLetterAndNumber));

        // Act
        ActionResult<LoginResponseDto> rootResult = await authController.AdminLogin(loginRequestDto);

        Assert.IsType<ObjectResult>(rootResult.Result);

        var result = rootResult?.Result as ObjectResult;
        Assert.Equal((int)ErrorCode.BadRequest, result!.StatusCode);
        
        // Assert that BadRequest response is returned
    }

    [Fact]
    public async Task AdminRegister_ValidCredentials_ReturnsOk_When_Admin_LoggedIn() {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var authDomainMock = new Mock<IAuthenticationDomain>();

        AuthController authController = new AuthController(configurationMock.Object, authDomainMock.Object);

        // Arrange with a logged in admin

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim(ClaimTypes.Email, "admin@admin.com"),
            new Claim(ClaimTypes.Role, "Admin")

        }, "mock"));

        authController.ControllerContext = new ControllerContext() {
            HttpContext = new DefaultHttpContext() { User = user }
        };


        var registerAdminRequest = new RegisterAdminRequestDto("test@example.dk", "password12");
        configurationMock.Setup(x => x["JWT:SecretKey"]).Returns("your_secret_key_that_is_not_too_short_and_is_too_long_2-3-3-4-a-sdasdasdasdasdasdas");
        configurationMock.Setup(x => x["JWT:Issuer"]).Returns("your_issuer");
        configurationMock.Setup(x => x["JWT:Audience"]).Returns("your_audience");
        configurationMock.Setup(x => x["JWT:Subject"]).Returns("your_subject");   
        
        authDomainMock.Setup(a => a.RegisterAdmin(registerAdminRequest)).Returns(Task.CompletedTask);

        // Act
        ActionResult rootResult = await authController.RegisterAdmin(registerAdminRequest);

        
        // Assert that Ok response is returned
        Assert.IsType<OkResult>(rootResult);
    }


    [Fact]
    public async Task AdminRegister_Fails_When_Admin_NotLoggedIn() {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var authDomainMock = new Mock<IAuthenticationDomain>();

        AuthController authController = new AuthController(configurationMock.Object, authDomainMock.Object);


        var registerAdminRequest = new RegisterAdminRequestDto("test@example.dk", "password12");
        configurationMock.Setup(x => x["JWT:SecretKey"]).Returns("your_secret_key_that_is_not_too_short_and_is_too_long_2-3-3-4-a-sdasdasdasdasdasdas");
        configurationMock.Setup(x => x["JWT:Issuer"]).Returns("your_issuer");
        configurationMock.Setup(x => x["JWT:Audience"]).Returns("your_audience");
        configurationMock.Setup(x => x["JWT:Subject"]).Returns("your_subject");   
        
        authDomainMock.Setup(a => a.RegisterAdmin(registerAdminRequest)).Returns(Task.CompletedTask);

        // Act
        ActionResult rootResult = await authController.RegisterAdmin(registerAdminRequest);

        
        // Assert that Ok response is returned
        Assert.IsType<OkResult>(rootResult);
    }
}