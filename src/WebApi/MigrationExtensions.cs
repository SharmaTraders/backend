using Microsoft.EntityFrameworkCore;

namespace WebApi;

public static class MigrationExtensions {
    public static void ApplyMigrations(this IApplicationBuilder app) {
        using var serviceScope = app.ApplicationServices.CreateScope();
        using DbContext context = serviceScope.ServiceProvider.GetRequiredService<DbContext>();
        context.Database.Migrate();
    }
}