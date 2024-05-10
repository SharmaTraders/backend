using System.Net;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;
using QueryContracts.item;
using WebApi.Endpoints.command.item;

namespace IntegrationTests.Item;

public class GetItemTests : BaseIntegrationTest {

    public GetItemTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task GetItems_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/item");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetItems_NoItems_ReturnsAnEmptyList() {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<GetAllItems.Answer>(responseContent);
        Assert.NotNull(items);
        Assert.Empty(items.Items);
    }

    [Fact]
    public async Task GetItem_ItemExistsInDatabase_ReturnsListOfItems() {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When there are items
        List<CreateItemRequest> requests = ItemFactory.GetCreateItemRequestsList();
        await SeedData.SeedItem(WriteDbContext, requests);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<GetAllItems.Answer>(responseContent);
        Assert.NotNull(items);
        Assert.Equal(requests.Count, items.Items.Count);
    }
}