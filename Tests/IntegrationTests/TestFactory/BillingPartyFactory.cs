using Dto;

namespace IntegrationTests.TestFactory;

public static class BillingPartyFactory {
    public static CreateBillingPartyRequestDto GetValidCreateBillingPartyRequestDto() {
        return new CreateBillingPartyRequestDto("Test Name", "Test Address", "1234567890", 100.0, "test@test.com",
            "15A26C23");
    }

    public static CreateBillingPartyRequestDto GetInvalidCreateBillingPartyRequestDto() {
        return new CreateBillingPartyRequestDto("", "", "", 0.0, "", "");
    }
}