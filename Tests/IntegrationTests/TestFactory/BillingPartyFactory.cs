using WebApi.Endpoints.command.billingParty;

namespace IntegrationTests.TestFactory;

public static class BillingPartyFactory {
    public static CreateBillingPartyRequest GetValidCreateBillingPartyRequestDto() {
        return new CreateBillingPartyRequest() {
            RequestBody = new CreateBillingPartyRequest.Body("Test Name", "Test Address", "1234567890", 100.0,
                "test@test.com",
                "15A26C23")
        };
    }

    public static List<CreateBillingPartyRequest> GetCreateBillingPartyRequestsList() {
        return [
            GetValidCreateBillingPartyRequestDto(),
            new CreateBillingPartyRequest() {
                RequestBody = new CreateBillingPartyRequest.Body("Test Name 2", "Test Address 2", "0011223344", 100.0,
                    "test2@test2.com",
                    "SOM342AXX")
            }
        ];
    }

    public static CreateBillingPartyRequest GetInvalidCreateBillingPartyRequestDto() {
        return new CreateBillingPartyRequest() {
            RequestBody = new CreateBillingPartyRequest.Body("", "", "", 0.0, "", "")
        };
    }

    public static UpdateBillingPartyRequest GetValidUpdateBillingPartyRequestDto() {
        return new UpdateBillingPartyRequest() {
            RequestBody = new UpdateBillingPartyRequest.Body("Test Name", "Test Address", "1234567890",
                "hello@gmail.com", "15A26C23")
        };
    }
}