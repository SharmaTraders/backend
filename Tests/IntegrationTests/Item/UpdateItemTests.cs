using System.Net;
using System.Text;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegrationTests.Item;

[Collection("Sequential")]
public class UpdateItemTests
{
    private readonly WebApp _application = new();

    [Fact]
    public async Task UpdateItem_NoToken_Fails()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "/Item/1");
        request.Content = new StringContent(JsonConvert.SerializeObject(ItemFactory.GetValidItemDto()),
            System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateItem_InvalidGUID_WithAdminToken_Fails()
    {
        string invalidId = "not-a-guid";
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Put, $"/Item/{invalidId}");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        UpdateItemRequest requestRequest = new("newName");
        request.Content = new StringContent(JsonConvert.SerializeObject(requestRequest), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Deserialize the response
        var responseContent = await response.Content.ReadAsStringAsync();
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(ErrorMessages.IdInvalid, problemDetails.Detail);
        Assert.Equal("Id", problemDetails.Type);
    }

    [Fact]
    public async Task UpdateItem_ItemDoesNotExist_WithAdminToken_Fails()
    {
        Guid validIdGuid = Guid.NewGuid();
        string validId = validIdGuid.ToString();
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        var request = new HttpRequestMessage(HttpMethod.Put, $"/Item/{validId}");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        UpdateItemRequest requestRequest = new("newName");
        request.Content = new StringContent(JsonConvert.SerializeObject(requestRequest), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Deserialize the response
        var responseContent = await response.Content.ReadAsStringAsync();
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(ErrorMessages.ItemNotFound(validIdGuid), problemDetails.Detail);
        Assert.Equal("Id", problemDetails.Type);
    }

    [Fact]
    public async Task UpdateItem_ValidItemName_WithAdminToken_Succeeds()
    {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        // When there are items
        List<CreateItemRequest> requests = ItemFactory.GetCreateItemRequestsList();
        await SeedData.SeedItem(_application, requests);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/Item");
        getRequest.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var client = _application.CreateClient();

        // Act on get request to retrieve items
        var getResponse = await client.SendAsync(getRequest);

        // Deserialize the get response
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<GetItemsResponse>(getResponseContent);

        // Check if items are returned
        Assert.NotNull(items);
        Assert.NotEmpty(items.Items);

        var firstItemId = items.Items.First().Id;

        // Create a new request to update the first item
        var updateRequest = new HttpRequestMessage(HttpMethod.Put, $"/Item/{firstItemId}");
        updateRequest.Headers.Add("Authorization", "Bearer " + validAdminToken);
        UpdateItemRequest updateRequestContent = new("newName");
        updateRequest.Content = new StringContent(JsonConvert.SerializeObject(updateRequestContent), Encoding.UTF8,
            "application/json");

        // Act on update request
        var updateResponse = await client.SendAsync(updateRequest);

        // Assert the update was successful
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateItem_ItemNameAlreadyExists_WithAdminToken_Fails()
    {
        // Arrange a logged in admin
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        // When there are items
        List<CreateItemRequest> requests = ItemFactory.GetCreateItemRequestsList();
        await SeedData.SeedItem(_application, requests);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/Item");
        getRequest.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var client = _application.CreateClient();

        // Act on get request to retrieve items
        var getResponse = await client.SendAsync(getRequest);

        // Deserialize the get response
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<GetItemsResponse>(getResponseContent);

        // Check if items are returned
        Assert.NotNull(items);
        Assert.True(items.Items.Count > 1, "There should be at least two items to test name collision properly.");

        var firstItem = items.Items.First();
        var lastItem = items.Items.Last();

        // Ensure first and last items are not the same
        Assert.NotEqual(firstItem.Id, lastItem.Id);

        // Create a new request to update the first item to have the same name as the last item
        var updateRequest = new HttpRequestMessage(HttpMethod.Put, $"/Item/{firstItem.Id}");
        updateRequest.Headers.Add("Authorization", "Bearer " + validAdminToken);
        UpdateItemRequest
            updateRequestContent = new(lastItem.Name); // Setting the first item's name to the last item's name
        updateRequest.Content = new StringContent(JsonConvert.SerializeObject(updateRequestContent), Encoding.UTF8,
            "application/json");

        // Act on update request
        var updateResponse = await client.SendAsync(updateRequest);

        // Deserialize the response
        var responseContent = await updateResponse.Content.ReadAsStringAsync();
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);

        // Assert that the update fails due to name conflict
        Assert.Equal(HttpStatusCode.Conflict, updateResponse.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Equal("ItemName", problemDetails.Type);
        Assert.Equal(ErrorMessages.ItemNameAlreadyExists(lastItem.Name), problemDetails.Detail);
    }
    
    
}