using Data;
using Domain.Entity;
using WebApi.Endpoints.command.billingParty;
using WebApi.Endpoints.command.item;

namespace IntegrationTests.Abstractions;

internal static class SeedData {
    public static async Task SeedAdmin(WriteDatabaseContext dbContext, AdminEntity adminEntity) {
        await dbContext.Admins.AddAsync(adminEntity);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedItem(WriteDatabaseContext dbContext ,CreateItemRequest createItemRequest) {
        ItemEntity entity = FromCreateRequest(createItemRequest);

        await dbContext.Items.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedItem(WriteDatabaseContext dbContext,  List<CreateItemRequest> createItemRequests) {
        List<ItemEntity> entities = createItemRequests.Select(FromCreateRequest).ToList();

        await dbContext.Items.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    public static Task SeedBillingParty(WriteDatabaseContext dbContext,  CreateBillingPartyRequest request) {


        BillingPartyEntity entity = FromCreateRequest(request);
        return SeedBillingParty(dbContext, entity);
    }

    public static async Task SeedBillingParty(WriteDatabaseContext dbContext,  BillingPartyEntity billingParty) {

        await dbContext.BillingParties.AddAsync(billingParty);
        await dbContext.SaveChangesAsync();
    }


    public static async Task SeedBillingParty(WriteDatabaseContext dbContext,  List<CreateBillingPartyRequest> billingParty) {

        List<BillingPartyEntity> entities = billingParty.Select(FromCreateRequest).ToList();

        await dbContext.BillingParties.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }


    private static ItemEntity FromCreateRequest(CreateItemRequest request) {
        return new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = request.RequestBody.Name,
            CurrentStockAmount = request.RequestBody.StockWeight ?? 0,
            CurrentEstimatedStockValuePerKilo = request.RequestBody.EstimatedPricePerKilo ?? 0
        };
    }

    private static BillingPartyEntity FromCreateRequest(CreateBillingPartyRequest request) {
        BillingPartyEntity entity = new BillingPartyEntity() {
            Id = Guid.NewGuid(),
            Name = request.RequestBody.Name,
            Address = request.RequestBody.Address,
            PhoneNumber = request.RequestBody.PhoneNumber,
            Balance = request.RequestBody.OpeningBalance ?? 0,
            Email = request.RequestBody.Email,
            VatNumber = request.RequestBody.VatNumber
        };
        return entity;
    }
}