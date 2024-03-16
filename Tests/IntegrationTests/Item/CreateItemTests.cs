using System.Net;
using Domain.utils;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;

namespace IntegrationTests.Item;

public class CreateItemTests {

    private readonly WebApp _application = new();

    [Fact]
    public async Task CreateItem_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Post, "/Item");
        request.Content = new StringContent(JsonConvert.SerializeObject(ItemFactory.GetValidItemDto()), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]

    public async Task CreateItem_ValidItemName_WithAdminToken_Succeeds(string itemName) {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Post, "/Item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        ItemDto requestDto = new(itemName);
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        string responseContent= await response.Content.ReadAsStringAsync();
        ItemDto? responseDto = JsonConvert.DeserializeObject<ItemDto>(responseContent);

        Assert.NotNull(responseDto);
        Assert.Equal(itemName, responseDto.Name);

    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInValidItemNames), MemberType = typeof(ItemFactory))]

    public async Task CreateItem_InValidItemName_WithAdminToken_Fails(string itemName) {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Post, "/Item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        ItemDto requestDto = new(itemName);
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]

    public async Task CreateItem_ValidItemNameThatAlreadyExists_WithAdminToken_Fails(string itemName) {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);
        ItemDto itemDto = new ItemDto(itemName);

        // Make sure the item already exists in the database
        await SeedData.SeedItem(_application, itemDto);

        var request = new HttpRequestMessage(HttpMethod.Post, "/Item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When sent a new item with the same name
        ItemDto requestDto = new(itemName);
        request.Content = new StringContent(JsonConvert.SerializeObject(requestDto), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        var errorContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(errorContent);
        Assert.Equal(ErrorMessages.ItemNameAlreadyExists, errorContent);
    }



}