using System.Net;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;

namespace IntegrationTests.Item;

[Collection("Sequential")]
public class GetItemsTest
{
    private readonly WebApp _webApp = new();

    [Fact]
    public async Task GetItems_NoToken_Fails()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/item");
        var client = _webApp.CreateClient();
        
        // Act
        var response = await client.SendAsync(request);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task GetItems_NoItems_ReturnsEmptyList()
    {
        // Arrange
        String validAdminToken = await UserFactory.SetupLoggedInAdmin(_webApp);
        
        var request = new HttpRequestMessage(HttpMethod.Get, "/item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        var client = _webApp.CreateClient();
        
        // Act
        var response = await client.SendAsync(request);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var itemsResponse = JsonConvert.DeserializeObject<GetItemsResponse>(responseContent);
        Assert.NotNull(itemsResponse.Items);
        Assert.Empty(itemsResponse.Items);
    }
    
    [Fact]
    public async Task GetItems_WithItems_ReturnsItems()
    {
        // Arrange
        String validAdminToken = await UserFactory.SetupLoggedInAdmin(_webApp);
        
        var request = new HttpRequestMessage(HttpMethod.Get, "/item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
        
        // seed database with items
        List<ItemDto> itemDtos = ItemFactory.GetValidItemsList();
        await SeedData.SeedItems(_webApp, itemDtos);
        
        var client = _webApp.CreateClient();
        
        // Act
        var response = await client.SendAsync(request);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var itemsResponse = JsonConvert.DeserializeObject<GetItemsResponse>(responseContent);
        Assert.NotNull(itemsResponse.Items);
        Assert.NotEmpty(itemsResponse.Items);
        Assert.Equal(itemDtos.Count, itemsResponse.Items.Count);
        Assert.Equal(itemDtos, itemsResponse.Items);
    }
}