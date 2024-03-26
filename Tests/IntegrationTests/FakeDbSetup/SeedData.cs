using Data;
using Data.converters;
using Data.Entities;
using Dto;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.FakeDbSetup;

internal static class SeedData {
    public static async Task SeedAdmin(WebApp application, AdminDto adminDto) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        AdminEntity entity = new AdminEntity() {
            Id = Guid.NewGuid(),
            Email = adminDto.Email,
            Password = HashPassword(adminDto.Password)
        };

        await dbContext.Admins.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedItem(WebApp application, AddItemRequest addItemRequest) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        ItemEntity entity = new ItemEntity() {
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