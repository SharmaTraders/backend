using System.Net;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;
using QueryContracts.billingParty;
using WebApi.Endpoints.command.billingParty;

namespace IntegrationTests.BillingParty;

public class GetBillingPartyTests : BaseIntegrationTest{

    public GetBillingPartyTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task GetBillingParties_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/billingParty");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetBillingParties_NoBillingParty_ReturnsAnEmptyList() {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/billingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var billingParties = JsonConvert.DeserializeObject<GetAllBillingParties.Answer>(responseContent);
        Assert.NotNull(billingParties);
        Assert.Empty(billingParties.Parties);
    }

    [Fact]
    public async Task GetBillingParty_BillingPartyExistsInDatabase_ReturnsListOfBillingParties() {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/billingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When there are billing parties
        List<CreateBillingPartyRequest> requests = BillingPartyFactory.GetCreateBillingPartyRequestsList();
        await SeedData.SeedBillingParty(WriteDbContext, requests);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var billingParties = JsonConvert.DeserializeObject<GetAllBillingParties.Answer>(responseContent);
        Assert.NotNull(billingParties);
        Assert.Equal(requests.Count, billingParties.Parties.Count);
    }
}