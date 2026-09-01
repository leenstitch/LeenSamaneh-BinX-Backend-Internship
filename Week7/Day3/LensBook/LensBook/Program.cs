using LensBook.DATA;
using LensBook.DATA.Seed;
using LensBook.Extensions;
using LensBook.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// =====================================================
// Add services to the container
// =====================================================

builder.Services.AddControllers();


// =====================================================
// Database
// =====================================================

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration
                .GetConnectionString(
                    "DefaultConnection")));


// =====================================================
// Authentication + Identity + JWT
// =====================================================

builder.Services.AddApplicationAuthentication(
    builder.Configuration);


// =====================================================
// Application Services
// =====================================================

builder.Services.AddApplicationServices();
builder.Services.AddHttpContextAccessor();

// =====================================================
// Swagger
// =====================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// =====================================================
// Build application
// =====================================================

var app = builder.Build();


// =====================================================
// Seed Identity Data
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager =
        services.GetRequiredService<
            UserManager<ApplicationUser>>();

    var roleManager =
        services.GetRequiredService<
            RoleManager<IdentityRole<int>>>();

    await IdentitySeeder.SeedAsync(
        userManager,
        roleManager);
}


// =====================================================
// HTTP Request Pipeline
// =====================================================

app.UseHttpsRedirection();


// Authentication MUST come before Authorization

app.UseAuthentication();

app.UseAuthorization();


// =====================================================
// Swagger
// =====================================================

app.UseSwagger();

app.UseSwaggerUI();


// =====================================================
// Controllers
// =====================================================

app.MapControllers();


// =====================================================
// Run
// =====================================================

app.Run();


// =====================================================
// Program class
// =====================================================

public partial class Program
{
}