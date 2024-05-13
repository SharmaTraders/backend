using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using Newtonsoft.Json;
using QueryContracts.item;

namespace IntegrationTests.Item.stock;

public class GetStocksTests : BaseIntegrationTest{
    public GetStocksTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task GetStocks_NoTokenFails() {
        Guid itemId = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/item/{itemId.ToString()}/stocks");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetStocks_WhenItemDoesNotExist_ReturnsEmptyList() {
        string validAdminToken = await SetupLoggedInAdmin();
        Guid itemId = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/item/{itemId.ToString()}/stocks");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        GetStocksByItem.Answer? responseBody = JsonConvert.DeserializeObject<GetStocksByItem.Answer>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(responseBody);
        Assert.Empty(responseBody.Stocks);
    }

    [Fact]
    public async Task GetStocks_WhenItemExists_AndStocksDoesNotExists_ReturnsSuccessWithEmptyList() {
        ItemEntity itemEntity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            CurrentStockAmount = 0,
            CurrentEstimatedStockValuePerKilo = 0
        };
        WriteDbContext.Items.Add(itemEntity);
        await WriteDbContext.SaveChangesAsync();

        string validAdminToken = await SetupLoggedInAdmin();
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/item/{itemEntity.Id.ToString()}/stocks");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        GetStocksByItem.Answer? responseBody = JsonConvert.DeserializeObject<GetStocksByItem.Answer>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(responseBody);
        Assert.Empty(responseBody.Stocks);
    }

 
    [Fact]
    public async Task GetStocks_WhenItemExists_AndStocksAlsoExists_ReturnsSuccessWithEmptyList() {
        ItemEntity itemEntity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            CurrentStockAmount = 0,
            CurrentEstimatedStockValuePerKilo = 0
        };
        Stock stock1 = new Stock() {
            Date = DateOnly.FromDateTime(DateTime.Now),
            EntryCategory = StockEntryCategory.AddStockEntry,
            Weight = 5,
            ExpectedValuePerKilo = 5,
            Remarks = "Remarks"
        };

        Stock stock2 = new Stock() {
            Date = DateOnly.FromDateTime(DateTime.Now),
            EntryCategory = StockEntryCategory.AddStockEntry,
            Weight = 10,
            ExpectedValuePerKilo = 5,
        };
        itemEntity.AddStock(stock1);
        itemEntity.AddStock(stock2);

        WriteDbContext.Items.Add(itemEntity);
        await WriteDbContext.SaveChangesAsync();

        string validAdminToken = await SetupLoggedInAdmin();
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/item/{itemEntity.Id.ToString()}/stocks");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        GetStocksByItem.Answer? responseBody = JsonConvert.DeserializeObject<GetStocksByItem.Answer>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(responseBody);
        Assert.NotEmpty(responseBody.Stocks);
        Assert.Equal(2, responseBody.Stocks.Count);
    }
}