// This file configures and starts the ASP.NET Core Web API.
// It registers the database, authentication, application services,
// Swagger documentation, middleware, controllers, and database seeding.

using System.Text;
using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Extensions;
using Cardiac_Patient_Monitoring_System.Middleware;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApplicationAuthentication(
    builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();

// Configures Swagger/OpenAPI and adds JWT authentication
// support for testing protected endpoints.
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
    });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

var app = builder.Build();

// Configures the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

// Handles unhandled exceptions and returns a consistent
// error response instead of exposing internal details.
app.UseMiddleware<GlobalExceptionMiddleware>();

// Enables JWT authentication.
app.UseAuthentication();

// Enables role-based and policy-based authorization.
app.UseAuthorization();

// Maps controller routes to the HTTP pipeline.
app.MapControllers();

// Seeds default roles and the Admin account when the application
// is not running in the Testing environment.
if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        await DbSeeder.SeedRolesAsync(services);
        await DbSeeder.SeedAdminAsync(services);
    }
}

app.Run();

// Makes the Program class accessible to WebApplicationFactory
// during integration testing.
public partial class Program
{
}