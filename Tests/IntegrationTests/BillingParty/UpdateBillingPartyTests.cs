using System.Net;
using Domain.Entity;
using Dto;
using IntegrationTests.FakeDbSetup;
using IntegrationTests.TestFactory;
using Newtonsoft.Json;

namespace IntegrationTests.BillingParty;

[Collection("Sequential")]
public class UpdateBillingPartyTests {

    private readonly WebApp _application = new();

    [Fact]
    public async Task UpdateBillingParty_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Put, "/BillingParty/1");
        request.Content = new StringContent(
            JsonConvert.SerializeObject(BillingPartyFactory.GetValidUpdateBillingPartyRequestDto()),
            System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task UpdateBillingParty_ValidBillingParty_PartyDoesNotExist_Fails() {

        // Arrange, a valid update request to non existing party
        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);
        string randomGuid = Guid.NewGuid().ToString();
        var request = new HttpRequestMessage(HttpMethod.Put, $"/BillingParty/{randomGuid}");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        request.Content = new StringContent(JsonConvert.SerializeObject(BillingPartyFactory.GetValidUpdateBillingPartyRequestDto()), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

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
        await SeedData.SeedBillingParty(_application,entity);

        string validAdminToken = await UserFactory.SetupLoggedInAdmin(_application);

        string id = entity.Id.ToString();
        var request = new HttpRequestMessage(HttpMethod.Put, $"/BillingParty/{id}");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When some fields are updated
        UpdateBillingPartyRequest updateBillingPartyRequest =
            new UpdateBillingPartyRequest("Sachin", "TestAddress", "", "testEmail@email.com", "");

        request.Content = new StringContent(JsonConvert.SerializeObject(updateBillingPartyRequest), System.Text.Encoding.UTF8,
            "application/json");

        var client = _application.CreateClient();

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    
}