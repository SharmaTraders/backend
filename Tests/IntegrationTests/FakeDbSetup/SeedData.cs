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

    public static async Task SeedItems(WebApp webApp, List<ItemDto> itemDtos)
    {
        using var scope = webApp.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        
        List<ItemEntity> entities = itemDtos.Select(dto => new ItemEntity() {Name = dto.Name}).ToList();
        
        await dbContext.Items.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync(); 
    }
}