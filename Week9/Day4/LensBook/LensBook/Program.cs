using System.Reflection;
using LensBook.DATA;
using LensBook.DATA.Seed;
using LensBook.Examples;
using LensBook.Extensions;
using LensBook.Middleware;
using LensBook.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

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
                    "DefaultConnection"))
                      .LogTo(
                Console.WriteLine,
                LogLevel.Information)
            .EnableSensitiveDataLogging());


//
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");
});
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
builder.Services.AddSwaggerExamplesFromAssemblyOf<RegisterCustomerExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateBookingExample>(); 
builder.Services.AddSwaggerExamplesFromAssemblyOf<AuthResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<BookingResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginExample>();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(
        AppContext.BaseDirectory,
        xmlFile);

    options.IncludeXmlComments(xmlPath);

    options.ExampleFilters();
});
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

//middleware for handling exceptions globally
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
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