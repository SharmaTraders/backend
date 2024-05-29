using System.Net;
using Domain.Entity;
using IntegrationTests.Abstractions;
using IntegrationTests.TestFactory;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Endpoints.command.expense;

namespace IntegrationTests.Expense
{
    public class ExpenseTests : BaseIntegrationTest
    {
        public ExpenseTests(IntegrationTestsWebAppFactory application) : base(application)
        {
        }

        [Fact]
        public async Task RegisterExpense_NoToken_Fails()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Content = new StringContent(
                JsonConvert.SerializeObject(ExpenseFactory.GetValidExpenseRequestForBillingParty().RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task RegisterExpense_WithInvalidCategoryRequest_Fails()
        {
            string validAdminToken = await SetupLoggedInAdmin();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            var invalidExpenseRequest = ExpenseFactory.GetInvalidExpenseRequestForCategory();
            request.Content = new StringContent(JsonConvert.SerializeObject(invalidExpenseRequest.RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task RegisterExpense_WhenBillingPartyDoesNotExists_Fails()
        {
            string validAdminToken = await SetupLoggedInAdmin();
            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            var validExpenseRequest = ExpenseFactory.GetValidExpenseRequestForBillingParty();
            request.Content = new StringContent(JsonConvert.SerializeObject(validExpenseRequest.RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task RegisterExpense_WhenEmployeeDoesNotExists_Fails()
        {
            string validAdminToken = await SetupLoggedInAdmin();
            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            var validExpenseRequest = ExpenseFactory.GetValidExpenseRequestForEmployee();
            request.Content = new StringContent(JsonConvert.SerializeObject(validExpenseRequest.RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task RegisterExpense_WithValidBillingPartyRequest_Succeeds()
        {
            string validAdminToken = await SetupLoggedInAdmin();
            BillingPartyEntity partyEntity = new BillingPartyEntity()
            {
                Id = Guid.NewGuid(),
                Address = "Test",
                Name = "Test",
                Balance = 500
            };
            await WriteDbContext.BillingParties.AddAsync(partyEntity);
            await WriteDbContext.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            var validExpenseRequest = new RegisterExpenseRequest()
            {
                RequestBody = new RegisterExpenseRequest.Body("2020-01-01", "Billing Party", 100, "Remarks",
                    partyEntity.Id.ToString(), null)
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(validExpenseRequest.RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var expenses = await ReadDbContext.Expenses.ToListAsync();
            Assert.NotEmpty(expenses);
            Assert.Single(expenses);

            var partyFromDb = await ReadDbContext.BillingParties.FindAsync(partyEntity.Id);
            Assert.NotNull(partyFromDb);
            Assert.Equal(partyEntity.Balance + validExpenseRequest.RequestBody.Amount, partyFromDb.Balance);
        }

        [Fact]
        public async Task RegisterExpense_WithValidEmployeeRequest_Succeeds()
        {
            string validAdminToken = await SetupLoggedInAdmin();
            EmployeeEntity employeeEntity = new EmployeeEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Test Employee",
                Address = "Test Address",
                NormalDailyWorkingMinute = 480,
                Status = EmployeeStatusCategory.Active,
                Balance = 200
            };
            await WriteDbContext.Employees.AddAsync(employeeEntity);
            await WriteDbContext.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            var validExpenseRequest = new RegisterExpenseRequest()
            {
                RequestBody = new RegisterExpenseRequest.Body("2020-01-01", "Salary", 50, "Remarks", null,
                    employeeEntity.Id.ToString())
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(validExpenseRequest.RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var expenses = await ReadDbContext.Expenses.ToListAsync();
            Assert.NotEmpty(expenses);
            Assert.Single(expenses);

            var employeeFromDb = await ReadDbContext.Employees.FindAsync(employeeEntity.Id);
            Assert.NotNull(employeeFromDb);
            Assert.Equal(employeeEntity.Balance - validExpenseRequest.RequestBody.Amount, employeeFromDb.Balance);
        }

        [Fact]
        public async Task RegisterExpense_WithValidCategoryRequest_Succeeds()
        {
            string validAdminToken = await SetupLoggedInAdmin();

            // Add the category to the database
            ExpenseCategoryEntity categoryEntity = new ExpenseCategoryEntity()
            {
                Name = "Food"
            };
            await WriteDbContext.ExpenseCategories.AddAsync(categoryEntity);
            await WriteDbContext.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/expenses");
            request.Headers.Add("Authorization", "Bearer " + validAdminToken);

            var validExpenseRequest = new RegisterExpenseRequest()
            {
                RequestBody =
                    new RegisterExpenseRequest.Body("2020-01-01", "Food", 150, "Dinner with clients", null, null)
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(validExpenseRequest.RequestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var expenses = await ReadDbContext.Expenses.ToListAsync();
            Assert.NotEmpty(expenses);
            Assert.Single(expenses);

            var expenseFromDb = await ReadDbContext.Expenses.FirstOrDefaultAsync(e => e.CategoryName == "Food");
            Assert.NotNull(expenseFromDb);
            Assert.Equal(validExpenseRequest.RequestBody.Amount, expenseFromDb.Amount);
            Assert.Equal(validExpenseRequest.RequestBody.Remarks, expenseFromDb.Remarks);
        }
    }
}