using System.Net;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Endpoints.command.item;

namespace IntegrationTests.Item;

public class CreateItemTests : BaseIntegrationTest {

    public CreateItemTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task CreateItem_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/item");
        request.Content = new StringContent(JsonConvert.SerializeObject(ItemFactory.GetValidCreateItemRequest().RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]
    public async Task CreateItem_ValidItemName_WithAdminToken_Succeeds(string itemName) {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Post, "api/item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        CreateItemRequest createItemRequest = new CreateItemRequest() {
            RequestBody = new CreateItemRequest.Body(itemName, 5,5)
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(createItemRequest.RequestBody), System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetInValidItemNames), MemberType = typeof(ItemFactory))]
    public async Task CreateItem_InValidItemName_WithAdminToken_Fails(string itemName) {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Post, "api/item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        CreateItemRequest createItemRequest = new CreateItemRequest() {
            RequestBody = new CreateItemRequest.Body(itemName, 5,5)
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(createItemRequest.RequestBody), System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(ItemFactory.GetValidItemNames), MemberType = typeof(ItemFactory))]
    public async Task CreateItem_ValidItemNameThatAlreadyExists_WithAdminToken_Fails(string itemName) {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();
        CreateItemRequest createItemRequest = new CreateItemRequest() {
            RequestBody = new CreateItemRequest.Body(itemName, 5,5)
        };

        // Make sure the item already exists in the database
        await SeedData.SeedItem(WriteDbContext, createItemRequest);

        var request = new HttpRequestMessage(HttpMethod.Post, "api/item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When sent a new item with the same name
        CreateItemRequest create = new CreateItemRequest() {
            RequestBody = new CreateItemRequest.Body(itemName, 5,5)
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(create.RequestBody), System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.ItemNameAlreadyExists(itemName), problemDetails.Detail);
    }
}