using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Endpoints.command.item;

namespace IntegrationTests.Item.stock;

public class ReduceStockTests : BaseIntegrationTest{

    public ReduceStockTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task RemoveStock_NoTokenFails() {
        Guid itemId = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/item/{itemId.ToString()}/reduce-stock");
        ReduceStockRequest reduceStockRequest = new ReduceStockRequest() {
            RequestBody = new ReduceStockRequest.Body(5, "2021-01-01", "Remarks")
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(reduceStockRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ReduceStock_WhenItemDoesNotExists_Fails() {
        string validAdminToken = await SetupLoggedInAdmin();
        Guid itemId = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/item/{itemId.ToString()}/reduce-stock");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        ReduceStockRequest reduceStockRequest = new ReduceStockRequest() {
            RequestBody = new ReduceStockRequest.Body(5, "2021-01-01", "Remarks")
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(reduceStockRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ReduceStock_WhenStockIsLessThanWeight_Fails() {
        ItemEntity itemEntity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            CurrentStockAmount = 0,
            CurrentEstimatedStockValuePerKilo = 0
        };
        WriteDbContext.Items.Add(itemEntity);
        await WriteDbContext.SaveChangesAsync();

        string validAdminToken = await SetupLoggedInAdmin();
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/item/{itemEntity.Id.ToString()}/reduce-stock");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When we try to reduce stock by 5, but the stock is 0
        ReduceStockRequest reduceStockRequest = new ReduceStockRequest() {
            RequestBody = new ReduceStockRequest.Body(5, "2021-01-01", "Remarks")
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(reduceStockRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

// Deserialize the response
        var responseContent = await response.Content.ReadAsStringAsync();
        ProblemDetails? problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
        Assert.NotNull(problemDetails);
        Assert.Equal(ErrorMessages.StockReduceCannotBeMoreThanCurrentStock, problemDetails.Detail);

    }

    [Fact]
    public async Task Reduce_WhenItemExists_ReducesStock() {

        ItemEntity itemEntity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            CurrentStockAmount = 15,
            CurrentEstimatedStockValuePerKilo = 0
        };
        WriteDbContext.Items.Add(itemEntity);
        await WriteDbContext.SaveChangesAsync();


        string validAdminToken = await SetupLoggedInAdmin();
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/item/{itemEntity.Id.ToString()}/reduce-stock");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        ReduceStockRequest reduceStockRequest = new ReduceStockRequest() {
            RequestBody = new ReduceStockRequest.Body(5, "2021-01-01", null)
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(reduceStockRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify
        var itemFromDb =await ReadDbContext.Items.FindAsync(itemEntity.Id);
        Assert.NotNull(itemFromDb);
        // Assert that the stock is decreased by 5
        Assert.Equal(10, itemFromDb.CurrentStockAmount);

    }
}