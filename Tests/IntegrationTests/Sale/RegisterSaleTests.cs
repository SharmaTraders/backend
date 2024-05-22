using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Endpoints.command.invoice.sale;

namespace IntegrationTests.Invoice.Sale
{
    public class RegisterSaleTests : BaseIntegrationTest
    {
        public RegisterSaleTests(IntegrationTestsWebAppFactory application) : base(application)
        {
        }
        
        [Fact]
        public async Task AddSale_NoTokenFails()
        {
            Guid billingPartyId = Guid.NewGuid();
            var request = new HttpRequestMessage(HttpMethod.Post, "api/sale");
            AddSaleRequest addSaleRequest = new AddSaleRequest()
            {
                RequestBody = new AddSaleRequest.Body(
                    billingPartyId.ToString(),
                    new List<AddSaleRequest.SaleLines>
                    {
                        new AddSaleRequest.SaleLines(Guid.NewGuid().ToString(), 1, 10, 0)
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
                JsonConvert.SerializeObject(addSaleRequest.RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            HttpResponseMessage response = await Client.SendAsync(request);

            // Assert that it is unauthorized
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        public async Task AddSale_WhenBillingPartyDoesNotExists_Fails()
        {
            string validAdminToken = await SetupLoggedInAdmin();
            Guid billingPartyId = Guid.NewGuid();
            var request = new HttpRequestMessage(HttpMethod.Post, "api/sale");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            AddSaleRequest addSaleRequest = new AddSaleRequest()
            {
                RequestBody = new AddSaleRequest.Body(
                    billingPartyId.ToString(),
                    new List<AddSaleRequest.SaleLines>
                    {
                        new AddSaleRequest.SaleLines(Guid.NewGuid().ToString(), 1, 10, 0)
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
                JsonConvert.SerializeObject(addSaleRequest.RequestBody),
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
        public async Task AddSale_WhenItemDoesNotExists_Fails()
        {
            // Arrange
            string validAdminToken = await SetupLoggedInAdmin();
            var billingParty = new BillingPartyEntity { Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address" };
        
            // adding valid billing party , so we can test the case when item does not exist
            WriteDbContext.BillingParties.Add(billingParty);
            await WriteDbContext.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/sale");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            AddSaleRequest addSaleRequest = new AddSaleRequest()
            {
                RequestBody = new AddSaleRequest.Body(
                    billingParty.Id.ToString(),
                    new List<AddSaleRequest.SaleLines>
                    {
                        new AddSaleRequest.SaleLines(Guid.NewGuid().ToString(), 1, 10, 0)
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
                JsonConvert.SerializeObject(addSaleRequest.RequestBody),
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
        public async Task AddSale_WhenItemAndBillingPartyExists_AddSale()
        {
            // Arrange
            string validAdminToken = await SetupLoggedInAdmin();
            Guid itemId = Guid.NewGuid();
            var billingParty = new BillingPartyEntity { Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address" };
            var item = new ItemEntity { Id = itemId, Name = "Valid Item", CurrentStockAmount = 100, CurrentEstimatedStockValuePerKilo = 50 };
        
            // adding valid billing party and item
            WriteDbContext.BillingParties.Add(billingParty);
            WriteDbContext.Items.Add(item);
            await WriteDbContext.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/sale");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            AddSaleRequest addSaleRequest = new AddSaleRequest()
            {
                RequestBody = new AddSaleRequest.Body(
                    billingParty.Id.ToString(),
                    new List<AddSaleRequest.SaleLines>
                    {
                        new AddSaleRequest.SaleLines(item.Id.ToString(), 1, 10, 0)
                    },
                    "2023-01-01",
                    null,
                    0,
                    0,
                    2,
                    null
                )
            };

            request.Content = new StringContent(
                JsonConvert.SerializeObject(addSaleRequest.RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            HttpResponseMessage response = await Client.SendAsync(request);

            // Assert that it is not found
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
            // Read the response content
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);

            // Parse the JSON response and extract the saleId
            var jsonResponse = JObject.Parse(responseContent);
            var saleIdString = jsonResponse["saleId"]?.ToString();
            Assert.NotNull(saleIdString);
            Assert.True(Guid.TryParse(saleIdString, out _));
            // check if the stock is reduced
            Query.Item? itemEntity = await ReadDbContext.Items.FindAsync(itemId);
            Assert.Equal(99, itemEntity.CurrentStockAmount);
            
            // check if the amount is added to billing party
            Query.BillingParty? billingPartyEntity = await ReadDbContext.BillingParties.FindAsync(billingParty.Id);
            Assert.Equal(8, billingPartyEntity.Balance);
        }
    }
}
