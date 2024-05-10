using System.Net;
using System.Text;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QueryContracts.item;
using WebApi.Endpoints.command.item;

namespace IntegrationTests.Item;

public class UpdateItemTests: BaseIntegrationTest 
{

    public UpdateItemTests(IntegrationTestsWebAppFactory application) : base(application) {
    }


    [Fact]
    public async Task UpdateItem_NoToken_Fails()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "api/item/1");
        request.Content = new StringContent(JsonConvert.SerializeObject(ItemFactory.GetValidCreateItemRequest()),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateItem_InvalidGUID_WithAdminToken_Fails()
    {
        string invalidId = "not-a-guid";
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();
    
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/item/{invalidId}");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);
    
        UpdateItemRequest requestRequest = new() {
            RequestBody = new UpdateItemRequest.Body("newName")
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(requestRequest.RequestBody), System.Text.Encoding.UTF8,
            "application/json");
    
        // Act
        HttpResponseMessage response = await Client.SendAsync(request);
    
        // Deserialize the response
        var responseContent = await response.Content.ReadAsStringAsync();
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(ErrorMessages.IdInvalid(invalidId), problemDetails.Detail);
        Assert.Equal("Id", problemDetails.Type);
    }

    [Fact]
    public async Task UpdateItem_ItemDoesNotExist_WithAdminToken_Fails()
    {
        Guid validIdGuid = Guid.NewGuid();
        string validId = validIdGuid.ToString();
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        var request = new HttpRequestMessage(HttpMethod.Put, $"api/item/{validId}");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        UpdateItemRequest requestRequest = new() {
            RequestBody = new UpdateItemRequest.Body("newName")
        };
        
        request.Content = new StringContent(JsonConvert.SerializeObject(requestRequest.RequestBody), System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
        string validAdminToken = await SetupLoggedInAdmin();

        // When there are items
        List<CreateItemRequest> requests = ItemFactory.GetCreateItemRequestsList();
        await SeedData.SeedItem(WriteDbContext, requests);

        var request = new HttpRequestMessage(HttpMethod.Get, "api/item");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Deserialize the get response
        var getResponseContent = await response.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<GetAllItems.Answer>(getResponseContent);

        // Check if items are returned
        Assert.NotNull(items);
        Assert.NotEmpty(items.Items);

        var firstItemId = items.Items.First().Id;

        // Create a new request to update the first item
        var updateRequest = new HttpRequestMessage(HttpMethod.Put, $"api/item/{firstItemId}");
        updateRequest.Headers.Add("Authorization", "Bearer " + validAdminToken);
        UpdateItemRequest requestRequest = new() {
            RequestBody = new UpdateItemRequest.Body("newName")
        };
        updateRequest.Content = new StringContent(JsonConvert.SerializeObject(requestRequest.RequestBody), Encoding.UTF8,
            "application/json");

        // Act on update request
        var updateResponse = await Client.SendAsync(updateRequest);

        // Assert the update was successful
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateItem_ItemNameAlreadyExists_WithAdminToken_Fails()
    {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        // When there are items
        List<CreateItemRequest> requests = ItemFactory.GetCreateItemRequestsList();
        await SeedData.SeedItem(WriteDbContext, requests);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/item");
        getRequest.Headers.Add("Authorization", "Bearer " + validAdminToken);


        // Act on get request to retrieve items
        var getResponse = await Client.SendAsync(getRequest);

        // Deserialize the get response
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        var items = JsonConvert.DeserializeObject<GetAllItems.Answer>(getResponseContent);

        // Check if items are returned
        Assert.NotNull(items);
        Assert.True(items.Items.Count > 1, "There should be at least two items to test name collision properly.");

        var firstItem = items.Items.First();
        var lastItem = items.Items.Last();

        // Ensure first and last items are not the same
        Assert.NotEqual(firstItem.Id, lastItem.Id);

        // Create a new request to update the first item to have the same name as the last item
        var updateRequest = new HttpRequestMessage(HttpMethod.Put, $"api/item/{firstItem.Id}");
        updateRequest.Headers.Add("Authorization", "Bearer " + validAdminToken);
        UpdateItemRequest requestRequest = new() {
            RequestBody = new UpdateItemRequest.Body(lastItem.Name)
        }; // Setting the first item's name to the last item's name
        updateRequest.Content = new StringContent(JsonConvert.SerializeObject(requestRequest.RequestBody), Encoding.UTF8,
            "application/json");

        // Act on update request
        var updateResponse = await Client.SendAsync(updateRequest);

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