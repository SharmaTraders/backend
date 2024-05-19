using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using Newtonsoft.Json;
using QueryContracts.income;

namespace IntegrationTests.Income;

public class GetIncomeByBillingPartyTests : BaseIntegrationTest {
    public GetIncomeByBillingPartyTests(IntegrationTestsWebAppFactory application) : base(application) {
    }

    [Fact]
    public async Task GetIncomeByBillingParty_NoToken_Fails() {
        Guid id = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/billing-party/{id.ToString()}/incomes");
        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert that it is unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetIncomeByBillingParty_NoBillingParty_ReturnsAnEmptyList() {
        // Arrange a logged in admin
        string validAdminToken = await SetupLoggedInAdmin();

        Guid id = Guid.NewGuid();
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/billing-party/{id.ToString()}/incomes");
        request.Headers.Add("Authorization", "Bearer " + validAdminToken);

        // Act
        HttpResponseMessage response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        string responseContent = await response.Content.ReadAsStringAsync();
        var incomes = JsonConvert.DeserializeObject<GetIncomeByBillingParty.Answer>(responseContent);
        Assert.NotNull(incomes);
        Assert.Empty(incomes.Incomes);
    }

    [Fact]
    public async Task GetIncomeByBillingParty_BillingPartyExistsInDatabase_HasNoIncome_ReturnsAnEmptyList() {

        BillingPartyEntity partyEntity = new BillingPartyEntity() {
            Id = Guid.NewGuid(),
            Name = "Test",
            Address = "Test"
        };
       await WriteDbContext.BillingParties.AddAsync(partyEntity);
       await WriteDbContext.SaveChangesAsync();

       string validAdminToken = await SetupLoggedInAdmin();
       var request  = new HttpRequestMessage(HttpMethod.Get, $"api/billing-party/{partyEntity.Id.ToString()}/incomes");
       request.Headers.Add("Authorization", "Bearer " + validAdminToken);

       // Act
       HttpResponseMessage response = await Client.SendAsync(request);
       Assert.Equal(HttpStatusCode.OK, response.StatusCode);

       GetIncomeByBillingParty.Answer? responseBody = JsonConvert.DeserializeObject<GetIncomeByBillingParty.Answer>(await response.Content.ReadAsStringAsync());
       Assert.NotNull(responseBody);
       Assert.Empty(responseBody.Incomes);

    }

    [Fact]
    public async Task GetIncomeByBillingParty_BillingPartyExistsInTheDatabase_AndHasIncome_ReturnsTheIncomeLists() {
        BillingPartyEntity partyEntity = new BillingPartyEntity() {
            Id = Guid.NewGuid(),
            Name = "Test",
            Address = "Test",
            Balance = 500
        };
       await WriteDbContext.BillingParties.AddAsync(partyEntity);
       await WriteDbContext.SaveChangesAsync();

       IncomeEntity incomeEntity1 = new IncomeEntity() {
           BillingParty = partyEntity,
           Amount = 100,
           Date = DateOnly.FromDateTime(DateTime.Now),
           Id = Guid.NewGuid(),
       };

       IncomeEntity incomeEntity2 = new IncomeEntity() {
           BillingParty = partyEntity,
           Amount = 200,
           Date = DateOnly.FromDateTime(DateTime.Now),
           Id = Guid.NewGuid(),
       };

       await WriteDbContext.Incomes.AddRangeAsync(incomeEntity1, incomeEntity2);
       await WriteDbContext.SaveChangesAsync();

       string validAdminToken = await SetupLoggedInAdmin();
       var request  = new HttpRequestMessage(HttpMethod.Get, $"api/billing-party/{partyEntity.Id.ToString()}/incomes");
       request.Headers.Add("Authorization", "Bearer " + validAdminToken);

       // Act
       HttpResponseMessage response = await Client.SendAsync(request);
       Assert.Equal(HttpStatusCode.OK, response.StatusCode);

       GetIncomeByBillingParty.Answer? responseBody = JsonConvert.DeserializeObject<GetIncomeByBillingParty.Answer>(await response.Content.ReadAsStringAsync());
       Assert.NotNull(responseBody);
       Assert.NotEmpty(responseBody.Incomes);
       Assert.Equal(2, responseBody.Incomes.Count);
    }

}