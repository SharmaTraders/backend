using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Query;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Abstractions;

public class IntegrationTestsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime {

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("SharmaTraders")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureTestServices(services => {
            services.RemoveAll(typeof(DbContextOptions<WriteDatabaseContext>));
            services.AddDbContext<WriteDatabaseContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            services.RemoveAll(typeof(DbContextOptions<SharmaTradersContext>));
            services.AddDbContext<SharmaTradersContext>(options => options
                .UseNpgsql(_dbContainer.GetConnectionString()));

        });
    }


    public async Task InitializeAsync() {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync() {
        await _dbContainer.StopAsync();
    }
}