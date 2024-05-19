using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Endpoints.command.invoice.purchase;

namespace IntegrationTests.Purchase;

public class AddPurchaseTests : BaseIntegrationTest {
    public AddPurchaseTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task AddPurchase_NoTokenFails() {
        Guid billingPartyId = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Post, "api/purchase");
        AddPurchaseRequest addPurchaseRequest = new AddPurchaseRequest() {
            RequestBody = new AddPurchaseRequest.Body(
                billingPartyId.ToString(),
                new List<AddPurchaseRequest.PurchaseLines> {
                    new AddPurchaseRequest.PurchaseLines(Guid.NewGuid().ToString(), 1, 10, 0)
                },
                "2023-01-01",
                null,
                0,
                0,
                0,
                null
            )
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(addPurchaseRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddPurchase_WhenBillingPartyDoesNotExists_Fails() {
        string validAdminToken = await SetupLoggedInAdmin();
        Guid billingPartyId = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Post, "api/purchase");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        AddPurchaseRequest addPurchaseRequest = new AddPurchaseRequest() {
            RequestBody = new AddPurchaseRequest.Body(
                billingPartyId.ToString(),
                new List<AddPurchaseRequest.PurchaseLines> {
                    new AddPurchaseRequest.PurchaseLines(Guid.NewGuid().ToString(), 1, 10, 0)
                },
                "2023-01-01",
                null,
                0,
                0,
                0,
                null
            )
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(addPurchaseRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert 
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // Assert that the response content contains the error message
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("BillingPartyId", responseContent);
    }


    [Fact]
    public async Task AddPurchase_WhenItemDoesNotExists_Fails() {
        // Arrange
        string validAdminToken = await SetupLoggedInAdmin();
        var billingParty = new BillingPartyEntity
            {Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address"};

        // adding valid billing party , so we can test the case when item does not exist
        WriteDbContext.BillingParties.Add(billingParty);
        await WriteDbContext.SaveChangesAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, "api/purchase");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        AddPurchaseRequest addPurchaseRequest = new AddPurchaseRequest() {
            RequestBody = new AddPurchaseRequest.Body(
                billingParty.Id.ToString(),
                new List<AddPurchaseRequest.PurchaseLines> {
                    new AddPurchaseRequest.PurchaseLines(Guid.NewGuid().ToString(), 1, 10, 0)
                },
                "2023-01-01",
                null,
                0,
                0,
                0,
                null
            )
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(addPurchaseRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is not found
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        // Read the response content
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("ItemId", responseContent);
    }

    [Fact]
    public async Task AddPurchase_WhenItemAndBillingPartyExists_AddPurchase() {
        // Arrange
        string validAdminToken = await SetupLoggedInAdmin();
        var billingParty = new BillingPartyEntity
            {Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address"};
        var item = new ItemEntity {
            Id = Guid.NewGuid(), Name = "Valid Item", CurrentStockAmount = 100, CurrentEstimatedStockValuePerKilo = 50
        };

        // adding valid billing party and item
        WriteDbContext.BillingParties.Add(billingParty);
        WriteDbContext.Items.Add(item);
        await WriteDbContext.SaveChangesAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, "api/purchase");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        AddPurchaseRequest addPurchaseRequest = new AddPurchaseRequest() {
            RequestBody = new AddPurchaseRequest.Body(
                billingParty.Id.ToString(),
                new List<AddPurchaseRequest.PurchaseLines> {
                    new AddPurchaseRequest.PurchaseLines(item.Id.ToString(), 1, 10, 0)
                },
                "2023-01-01",
                null,
                0,
                0,
                0,
                null
            )
        };

        request.Content = new StringContent(
            JsonConvert.SerializeObject(addPurchaseRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is not found
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Read the response content
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);

        // Parse the JSON response and extract the purchaseId
        var jsonResponse = JObject.Parse(responseContent);
        var purchaseIdString = jsonResponse["purchaseId"]?.ToString();
        Assert.NotNull(purchaseIdString);
        Assert.True(Guid.TryParse(purchaseIdString, out _));
    }
}