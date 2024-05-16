using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Endpoints.command.income;

namespace IntegrationTests.Income;

public class IncomeTests : BaseIntegrationTest {
    public IncomeTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task RegisterIncome_NoToken_Fails() {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/income");
        request.Content = new StringContent(
            JsonConvert.SerializeObject(IncomeFactory.GetValidIncomeRequest().RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RegisterIncome_WhenBillingPartyDoesNotExists_Fails() {
        string validAdminToken = await SetupLoggedInAdmin();
        var request = new HttpRequestMessage(HttpMethod.Post, "api/income");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        var validIncomeRequest = IncomeFactory.GetValidIncomeRequest();
        request.Content = new StringContent(JsonConvert.SerializeObject(validIncomeRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is bad request
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RegisterIncome_WithValidRequest_Succeeds() {
        string validAdminToken = await SetupLoggedInAdmin();
        BillingPartyEntity partyEntity = new BillingPartyEntity() {
            Id = Guid.NewGuid(),
            Address = "Test",
            Name = "Test",
            Balance = 500
        };
        await WriteDbContext.BillingParties.AddAsync(partyEntity);
        await WriteDbContext.SaveChangesAsync();


        var request = new HttpRequestMessage(HttpMethod.Post, "api/income");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // When adding an income of 100
        var validIncomeRequest = new RegisterIncomeRequest() {
            RequestBody = new RegisterIncomeRequest.Body("2020-01-01",
                partyEntity.Id.ToString(),
                100,
                "Remarks")
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(validIncomeRequest.RequestBody),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert 
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var lists =await ReadDbContext.Incomes.ToListAsync();
        Assert.NotEmpty(lists);
        Assert.Single(lists);


        // Assert that the party balance has changed..
        var partyFromDb = await ReadDbContext.BillingParties.FindAsync(partyEntity.Id);
        Assert.NotNull(partyFromDb);
        Assert.Equal(partyEntity.Balance - validIncomeRequest.RequestBody.Amount, partyFromDb.Balance);
    }
}