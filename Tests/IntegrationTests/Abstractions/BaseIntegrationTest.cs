using System.Net;
using Data;
using IntegrationTests.TestFactory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Query;
using WebApi.Endpoints.command.authentication;

namespace IntegrationTests.Abstractions;

public class BaseIntegrationTest : IClassFixture<IntegrationTestsWebAppFactory>, IDisposable {


    private readonly IServiceScope _scope;

    protected HttpClient Client { get; }

    protected WriteDatabaseContext WriteDbContext {
        get;
    }

    protected SharmaTradersContext ReadDbContext {
        get;
    }

 

    protected BaseIntegrationTest(IntegrationTestsWebAppFactory application) {
        _scope = application.Services.CreateScope();
        WriteDbContext = _scope.ServiceProvider.GetRequiredService<WriteDatabaseContext>();
        ReadDbContext = _scope.ServiceProvider.GetRequiredService<SharmaTradersContext>();
        WriteDbContext.Database.EnsureDeleted();
        WriteDbContext.Database.EnsureCreated();

        ReadDbContext.Database.EnsureDeleted();
        ReadDbContext.Database.EnsureCreated();

        Client = application.CreateClient();
    }


    protected  async Task<string> SetupLoggedInAdmin() {
        // Arrange an already existing admin in db
        var adminUser = UserFactory.GetValidAdminEntity();
        await SeedData.SeedAdmin(WriteDbContext, adminUser);

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login/admin");
        var loginRequestDto = new LoginAdminRequest() {
            RequestBody = new LoginAdminRequest.Body(adminUser.Email, "somePassword1234")
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(loginRequestDto.RequestBody), System.Text.Encoding.UTF8,
            "application/json");


        HttpResponseMessage response = await Client.SendAsync(request);
        // Assert  that the login succeeds
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert that the jwtToken is returned
        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponseDto = JsonConvert.DeserializeObject<LoginAdminResponse>(responseContent);

        Assert.NotNull(loginRequestDto);
        Assert.False(string.IsNullOrEmpty(loginResponseDto!.JwtToken));

        return loginResponseDto.JwtToken;
    }

    public void Dispose() {
        _scope.Dispose();
    }
}