using System.Reflection;
using System.Text;

using LensBook.Configuration;
using LensBook.DATA;
using LensBook.DATA.Seed;
using LensBook.Extensions;
using LensBook.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


// =====================================================
// Add services to the container
// =====================================================

builder.Services.AddControllers();


// =====================================================
// JWT Settings
// =====================================================

var jwtSettings =
    builder.Configuration.GetSection("Jwt");

var key =
    Encoding.UTF8.GetBytes(
        jwtSettings["Key"]!);

builder.Services
    .AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,

                    ValidateAudience = true,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    ValidIssuer =
                        jwtSettings["Issuer"],

                    ValidAudience =
                        jwtSettings["Audience"],

                    IssuerSigningKey =
                        new SymmetricSecurityKey(key),

                    ClockSkew =
                        TimeSpan.Zero
                };
        });


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
// ASP.NET Core Identity
// =====================================================

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// =====================================================
// JWT Settings Registration
// =====================================================

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));


// =====================================================
// Application Services
// =====================================================

builder.Services.AddApplicationServices();


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


// Swagger
app.UseSwagger();

app.UseSwaggerUI(
    options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "LensBook API V1");

        options.RoutePrefix = "swagger";
    });



    app.MapControllers();



// =====================================================
// Run
// =====================================================

app.Run();