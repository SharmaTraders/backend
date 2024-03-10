using Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi;

public static class MigrationExtensions {
    public static void ApplyMigrations(this IApplicationBuilder app) {
        using var serviceScope = app.ApplicationServices.CreateScope();
        using DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
        context.Database.Migrate();
    }
}