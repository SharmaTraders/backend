using Data;
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

        await dbContext.Admins!.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedItem(WebApp application, ItemDto itemDto) {
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        ItemEntity entity = new ItemEntity() {
            Name = itemDto.Name
        };

        await dbContext.Items!.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }


    private static string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}