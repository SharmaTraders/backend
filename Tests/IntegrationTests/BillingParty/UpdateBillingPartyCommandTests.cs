using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;
using WebApi.Endpoints.command.billingParty;

namespace IntegrationTests.BillingParty;

public class UpdateBillingPartyCommandTests : BaseIntegrationTest {

    public UpdateBillingPartyCommandTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task UpdateBillingParty_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Put, "api/billingParty/1");
        request.Content = new StringContent(
            JsonConvert.SerializeObject(BillingPartyFactory.GetValidUpdateBillingPartyRequestDto().RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task UpdateBillingParty_ValidBillingParty_PartyDoesNotExist_Fails() {
        // Arrange, a valid update request to non existing party
        string validAdminToken = await SetupLoggedInAdmin();
        string randomGuid = Guid.NewGuid().ToString();
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/billingParty/{randomGuid}");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        request.Content = new StringContent(
            JsonConvert.SerializeObject(BillingPartyFactory.GetValidUpdateBillingPartyRequestDto().RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBillingParty_ValidBillingParty_PartyExists_Succeeds() {
        BillingPartyEntity entity = new BillingPartyEntity() {
            Id = Guid.NewGuid(),
            Name = "TestName",
            Address = "TestAddress",
            Email = "TestEmail@email.com",
            PhoneNumber = "1234567890"
        };
        // When the party exists
        await SeedData.SeedBillingParty(WriteDbContext, entity);

        string validAdminToken = await SetupLoggedInAdmin();

        string id = entity.Id.ToString();
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/billingParty/{id}");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When some fields are updated
        UpdateBillingPartyRequest updateBillingPartyRequest =
            new UpdateBillingPartyRequest() {
                RequestBody = new UpdateBillingPartyRequest.Body("Sachin", "TestAddress", "", "testEmail@email.com", "")
            };

        request.Content = new StringContent(JsonConvert.SerializeObject(updateBillingPartyRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}