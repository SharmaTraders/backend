using Data;
using Domain.converters;
using Domain.Entity;
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

    public static async Task SeedItem(WebApp application, AddItemRequest addItemRequest) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        ItemEntity entity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = addItemRequest.Name
        };

        await dbContext.Items.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedBillingParty(WebApp application, CreateBillingPartyRequest billingParty) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        BillingPartyEntity entity = BillingPartyConverter.ToEntity(billingParty);

        await dbContext.BillingParties.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }


    public static async Task SeedBillingParty(WebApp application, List<CreateBillingPartyRequest> billingParty) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        List<BillingPartyEntity> entities = billingParty.Select(BillingPartyConverter.ToEntity).ToList();

        await dbContext.BillingParties.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    private static string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}