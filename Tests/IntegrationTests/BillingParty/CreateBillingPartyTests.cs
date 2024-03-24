using System.Net;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;

namespace IntegrationTests.BillingParty;

[Collection("Sequential")]
public class CreateBillingPartyTests {

    private readonly WebApp _application = new();

    [Fact]
    public async Task CreateBillingParty_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Post, "/BillingParty");
        request.Content = new StringContent(JsonConvert.SerializeObject(BillingPartyFactory.GetValidCreateBillingPartyRequestDto()), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateBillingParty_ValidBillingParty_Succeeds() {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Post, "/BillingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var requestDto = BillingPartyFactory.GetValidCreateBillingPartyRequestDto();
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateBillingParty_InvalidBillingParty_Fails() {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Post, "/BillingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var requestDto = BillingPartyFactory.GetInvalidCreateBillingPartyRequestDto();
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateBillingParty_ValidBillingParty_AlreadyExists_Fails() {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);
        CreateBillingPartyRequestDto requestDto = BillingPartyFactory.GetValidCreateBillingPartyRequestDto();

        await SeedData.SeedBillingParty(_application,requestDto);

        var request = new HttpRequestMessage(HttpMethod.Post, "/BillingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

}