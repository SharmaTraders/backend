using System.Net;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;

namespace IntegrationTests.BillingParty;

[Collection("Sequential")]
public class GetBillingPartyTests {

    private readonly WebApp _application = new ();

    [Fact]
    public async Task GetBillingParties_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Get, "/BillingParty");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetBillingParties_NoBillingParty_ReturnsAnEmptyList() {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Get, "/BillingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var billingParties = JsonConvert.DeserializeObject<GetBillingPartiesResponse>(responseContent);
        Assert.NotNull(billingParties);
        Assert.Empty(billingParties.BillingParties);
    }

    [Fact]
    public async Task GetBillingParty_BillingPartyExistsInDatabase_ReturnsListOfBillingParties() {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Get, "/BillingParty");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When there are billing parties
        List<CreateBillingPartyRequest> requests = BillingPartyFactory.GetCreateBillingPartyRequestsList();
        await SeedData.SeedBillingParty(_application, requests);

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var billingParties = JsonConvert.DeserializeObject<GetBillingPartiesResponse>(responseContent);
        Assert.NotNull(billingParties);
        Assert.Equal(requests.Count, billingParties.BillingParties.Count);
    }



}