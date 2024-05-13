using System.Net;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace IntegrationTests.BillingParty;

public class CreateTests : BaseIntegrationTest {
    private readonly ITestOutputHelper _testOutputHelper;

    public CreateTests(IntegrationTestsWebAppFactory application, ITestOutputHelper testOutputHelper) : base(application) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateBillingParty_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/billingParty");
        request.Content = new StringContent(
            JsonConvert.SerializeObject(BillingPartyFactory.GetValidCreateBillingPartyRequestDto().RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateBillingParty_ValidBillingParty_Succeeds() {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Post, "api/billingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var requestDto = BillingPartyFactory.GetValidCreateBillingPartyRequestDto();
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto.RequestBody), System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);
        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(requestDto.RequestBody, new JsonSerializerSettings() {
            Formatting = Formatting.Indented
        }));
        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(response, new JsonSerializerSettings() {
            Formatting = Formatting.Indented
        }));

        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(request, new JsonSerializerSettings() {
            Formatting = Formatting.Indented
        }));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateBillingParty_InvalidBillingParty_Fails() {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/billingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var requestDto = BillingPartyFactory.GetInvalidCreateBillingPartyRequestDto();
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto.RequestBody), System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateBillingParty_ValidBillingParty_AlreadyExists_Fails() {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();
        var createRequest = BillingPartyFactory.GetValidCreateBillingPartyRequestDto();

        await SeedData.SeedBillingParty(WriteDbContext, createRequest);

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/billingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        request.Content = new StringContent(JsonConvert.SerializeObject(createRequest.RequestBody), System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}