using System.Text;
using Application;
using Application.services.billingParty;
using Application.services.item;
using CommandContracts;
using Data;
using Data.Repository;
using Domain.common;
using Domain.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Query;
using Query.Services.billingParty;
using Query.Services.item;
using QueryContracts;
using WebApi;
using WebApi.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WriteDatabaseContext>(options =>
    options.UseNpgsql(connectionString!, optionsBuilder => optionsBuilder.EnableRetryOnFailure()));

builder.Services.AddDbContext<SharmaTradersContext>(options => {
    options.UseNpgsql(connectionString!, optionsBuilder => optionsBuilder.EnableRetryOnFailure());
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

});


// Repos
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IBillingPartyRepository, BillingPartyRepository>();


// Services
builder.Services.AddScoped<IUniqueBillingPartyNameChecker, UniqueBillingPartyNameChecker>();
builder.Services.AddScoped<IUniqueBillingPartyEmailChecker, UniqueBillingPartyEmailChecker>();
builder.Services.AddScoped<IUniqueBillingPartyVatNumberChecker, UniqueBillingPartyVatNumberChecker>();
builder.Services.AddScoped<IUniqueItemNameChecker, UniqueItemNameChecker>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Queries
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblies(QueryContractsAssembly.Assembly, QueryAssembly.Assembly));
// Register Commands
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblies(CommandContractsAssembly.Assembly, ApplicationAssembly.Assembly));

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!))
    };
});


builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() {Title = "WebApi", Version = "v1"});
    c.AddSecurityDefinition("Bearer", new() {
        Description = "Please issue Bearer token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new() {
        {
            new() {
                Reference = new() {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();             
app.UseCors(policyBuilder => policyBuilder.AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();


public partial class Program {

}
