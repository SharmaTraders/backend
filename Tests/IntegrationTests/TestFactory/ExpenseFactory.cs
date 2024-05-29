using WebApi.Endpoints.command.expense;

namespace IntegrationTests.TestFactory;

public static class ExpenseFactory
{
    public static RegisterExpenseRequest GetInvalidExpenseRequestForCategory()
    {
        return new RegisterExpenseRequest()
        {
            RequestBody = new RegisterExpenseRequest.Body(
                "2021-01-01",
                "NonExistentCategory",
                100,
                "Remarks",
                null,
                null)
        };
    }

    public static RegisterExpenseRequest GetValidExpenseRequestForBillingParty()
    {
        return new RegisterExpenseRequest()
        {
            RequestBody = new RegisterExpenseRequest.Body(
                "2021-01-01",
                "Billing Party",
                100,
                "Remarks",
                Guid.NewGuid().ToString(),
                null)
        };
    }

    public static RegisterExpenseRequest GetValidExpenseRequestForEmployee()
    {
        return new RegisterExpenseRequest()
        {
            RequestBody = new RegisterExpenseRequest.Body(
                "2021-01-01",
                "Salary",
                50,
                "Remarks",
                null,
                Guid.NewGuid().ToString())
        };
    }
}