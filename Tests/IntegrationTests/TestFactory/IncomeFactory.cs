using WebApi.Endpoints.command.income;

namespace IntegrationTests.TestFactory;

public static class IncomeFactory {
    public static RegisterIncomeRequest GetValidIncomeRequest() {
        return new RegisterIncomeRequest() {
            RequestBody = new RegisterIncomeRequest.Body("2021-01-01",
                Guid.NewGuid().ToString(),
                100,
                "Remarks")
        };
    }
}