using Data;
using Domain.Entity;
using DomainEntry.converters;
using Dto;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.FakeDbSetup;

internal static class SeedData {
    public static async Task SeedAdmin(WebApp application, AdminEntity adminEntity) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();


        await dbContext.Admins.AddAsync(adminEntity);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedItem(WebApp application, CreateItemRequest createItemRequest) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        ItemEntity entity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = createItemRequest.Name
        };

        await dbContext.Items.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedItem(WebApp application, List<CreateItemRequest> createItemRequests) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        List<ItemEntity> entities = createItemRequests.Select(request => new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = request.Name
        }).ToList();

        await dbContext.Items.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    public static Task SeedBillingParty(WebApp application, CreateBillingPartyRequest billingParty) {
      

        BillingPartyEntity entity = BillingPartyConverter.ToEntity(billingParty);

        return SeedBillingParty(application, entity);
    }

    public static async Task SeedBillingParty(WebApp application, BillingPartyEntity billingParty) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        await dbContext.BillingParties.AddAsync(billingParty);
        await dbContext.SaveChangesAsync();
    }


    public static async Task SeedBillingParty(WebApp application, List<CreateBillingPartyRequest> billingParty) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        List<BillingPartyEntity> entities = billingParty.Select(BillingPartyConverter.ToEntity).ToList();

        await dbContext.BillingParties.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }
}