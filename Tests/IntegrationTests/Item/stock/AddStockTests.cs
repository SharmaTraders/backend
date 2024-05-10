using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using Newtonsoft.Json;
using WebApi.Endpoints.command.item;

namespace IntegrationTests.Item.stock;

public class AddStockTests : BaseIntegrationTest {
    public AddStockTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task AddStock_NoTokenFails() {
        Guid itemId = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/item/{itemId.ToString()}/add-stock");
        AddStockRequest addStockRequest = new AddStockRequest() {
            RequestBody = new AddStockRequest.Body(5, 5, "2021-01-01")
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(addStockRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddStock_WhenItemDoesNotExists_Fails() {
        string validAdminToken = await SetupLoggedInAdmin();
        Guid itemId = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/item/{itemId.ToString()}/add-stock");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        AddStockRequest addStockRequest = new AddStockRequest() {
            RequestBody = new AddStockRequest.Body(5, 5, "2021-01-01")
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(addStockRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddStock_WhenItemExists_AddsStock() {

        ItemEntity itemEntity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            CurrentStockAmount = 0,
            CurrentEstimatedStockValuePerKilo = 0
        };
        WriteDbContext.Items.Add(itemEntity);
        await WriteDbContext.SaveChangesAsync();


        string validAdminToken = await SetupLoggedInAdmin();
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/item/{itemEntity.Id.ToString()}/add-stock");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        AddStockRequest addStockRequest = new AddStockRequest() {
            RequestBody = new AddStockRequest.Body(5, 5, "2021-01-01")
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(addStockRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify
        var itemFromDb =await ReadDbContext.Items.FindAsync(itemEntity.Id);
        Assert.NotNull(itemFromDb);
        Assert.Equal(5, itemFromDb.CurrentStockAmount);
        Assert.Equal(5, itemFromDb.CurrentEstimatedStockValuePerKilo);

    }
}