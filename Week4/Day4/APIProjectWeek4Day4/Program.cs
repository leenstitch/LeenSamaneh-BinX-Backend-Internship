using System.Text;
using APIProject.Data;
using APIProject.Extensions;
using APIProject.Interfaces;
using APIProject.Middleware;
using APIProject.Models;
using APIProject.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

//using APIProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// Create the WebApplication builder.
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//FluentValidation Configuration 
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();
builder.Services.AddFluentValidationAutoValidation();

//========== Identity Configuration ==========
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<LibraryDbContext>();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey!))
        };
    });


//========== Services Configuration ==========

// Register MVC Controllers in the Dependency Injection container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices();



// Register services required for generating API documentation.
builder.Services.AddEndpointsApiExplorer();

// Add Swagger generator service.
builder.Services.AddSwaggerGen();

//Custom Authorization Configuration 
builder.Services.AddCustomAuthorization();
// Build the WebApplication object.
var app = builder.Build();

// Database Initialization 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<LibraryDbContext>();

    context.Database.Migrate();
}

// Seed Roles into the database
using (var scope = app.Services.CreateScope())
{
    await Seeding.SeedRolesAsync(scope.ServiceProvider);
}

//========== Middleware Configuration ==========

// Check if the application is running in Development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Enable Swagger JSON endpoint.
    app.UseSwaggerUI(); // Enable Swagger UI page for testing API endpoints.
}


// Redirect HTTP requests to HTTPS for security.
app.UseHttpsRedirection();

//Using my own Middleware at the right place
app.UseMiddleware<RequestLoggingMiddleware>();

// Enables authentication middleware.
app.UseAuthentication();


// Enables authorization middleware.
app.UseAuthorization();









// Maps Controller routes to the application.
app.MapControllers();
//using my own  middleware at the wrong place
//app.UseMiddleware<RequestLoggingMiddleware>();
app.Run();