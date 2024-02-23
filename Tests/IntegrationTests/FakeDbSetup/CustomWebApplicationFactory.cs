using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests.FakeDbSetup;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program> {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureTestServices(services => {
            services.RemoveAll(typeof(DbContextOptions<DatabaseContext>));

            services.AddDbContext<DatabaseContext>(options => { options.UseInMemoryDatabase("InMemoryDbForTesting"); });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            DatabaseContext dbContext = scopedServices.GetRequiredService<DatabaseContext>();
            dbContext.Database.EnsureDeleted();
        });
    }

}