using System.Net;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;

namespace IntegrationTests.Item;

[Collection("Sequential")]
public class GetItemTests {
    private readonly WebApp _application = new();

    [Fact]
    public async Task GetItems_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Get, "/Item");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetItems_NoItems_ReturnsAnEmptyList() {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Get, "/Item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<GetItemsResponse>(responseContent);
        Assert.NotNull(items);
        Assert.Empty(items.Items);
    }

    [Fact]
    public async Task GetItem_ItemExistsInDatabase_ReturnsListOfItems() {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Get, "/Item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When there are items
        List<CreateItemRequest> requests = ItemFactory.GetCreateItemRequestsList();
        await SeedData.SeedItem(_application, requests);

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<GetItemsResponse>(responseContent);
        Assert.NotNull(items);
        Assert.Equal(requests.Count, items.Items.Count);
    }
}