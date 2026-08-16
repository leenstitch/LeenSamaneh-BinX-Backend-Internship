using System.Text;
using System.Threading.RateLimiting;
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
using Microsoft.AspNetCore.RateLimiting;
//using APIProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// Create the WebApplication builder.
var builder = WebApplication.CreateBuilder(args);


// ============================================================
// HSTS CONFIGURATION
// ============================================================

// Add HSTS services to the Dependency Injection container.
// ITS value is set to 30 days, and it includes subdomains for enhanced security.
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(30);
    options.IncludeSubDomains = true;
});

// ============================================================
// DATABASE CONFIGURATION
// ============================================================

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ============================================================
// FLUENTVALIDATION CONFIGURATION
// ============================================================
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();
builder.Services.AddFluentValidationAutoValidation();



// ============================================================
// IDENTITY CONFIGURATION
// ============================================================
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<LibraryDbContext>();


// ============================================================
// JWT CONFIGURATION
// ============================================================

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];



// ============================================================
// AUTHENTICATION CONFIGURATION
// ============================================================

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })

    // Configures JWT Bearer authentication.
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

// ============================================================
// RATE LIMITING CONFIGURATION
// ============================================================
builder.Services.AddRateLimiter(options =>
{
    // Define a general rate limiting policy for all requests.
    options.AddFixedWindowLimiter("GeneralPolicy", limiterOptions =>
    {
        // Allows a maximum of 100 requests.
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 0;
    });

    // Define a rate limiting policy for login request
    options.AddFixedWindowLimiter("LoginPolicy", limiterOptions =>
    {
        // Allows a maximum of 5 requests.
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 0;
    });

    // Returns HTTP 429 when the rate limit is exceeded.
    // 429 means "Too Many Requests".
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});



// ============================================================
// CORS CONFIGURATION
// ============================================================

// Adds CORS services to the application.
// CORS controls which frontend applications are allowed
// to communicate with our API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // Allows requests only from the frontend running on localhost:5173.
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod(); // Allows any HTTP methods such as GET, POST, PUT, and DELETE.
    });
});



// ============================================================
// SERVICES CONFIGURATION
// ============================================================

// Registers MVC Controllers in the Dependency Injection container.
// This allows the application to use API Controllers.
builder.Services.AddControllers();

// Registers the application's custom services.
builder.Services.AddApplicationServices();


// ============================================================
// SWAGGER CONFIGURATION
// ============================================================

// Adds API endpoint discovery services.
// Swagger uses this to discover Controllers and API endpoints.
builder.Services.AddEndpointsApiExplorer();


// Add Swagger generator service.
builder.Services.AddSwaggerGen();

// ============================================================
// CUSTOM AUTHORIZATION
// ============================================================

//Custom Authorization Configuration 
builder.Services.AddCustomAuthorization();


// ============================================================
// BUILD APPLICATION
// ============================================================

// Build the WebApplication object.
var app = builder.Build();

// ============================================================
// DATABASE MIGRATION
// ============================================================

// Database Initialization 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<LibraryDbContext>();

    context.Database.Migrate();
}

// ============================================================
// SEED ROLES
// ============================================================
using (var scope = app.Services.CreateScope())
{
    await Seeding.SeedRolesAsync(scope.ServiceProvider);
}


// ============================================================
// DEVELOPMENT ENVIRONMENT
// ============================================================

// Check if the application is running in Development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Enable Swagger JSON endpoint.

    app.UseSwaggerUI(); // Enable Swagger UI page for testing API endpoints.
}


// ============================================================
// HTTPS
// ============================================================
app.UseHttpsRedirection();


// ============================================================
// HSTS IN PRODUCTION
// ============================================================
if (!app.Environment.IsDevelopment())
{
    // Enables HSTS in the Production environment.
    // This tells browsers to always use HTTPS for the application
    app.UseHsts();
}

// ============================================================
// CUSTOM REQUEST LOGGING MIDDLEWARE
// ============================================================
app.UseMiddleware<RequestLoggingMiddleware>();


// ============================================================
// RATE LIMITER MIDDLEWARE
// ============================================================

// It must be in the HTTP pipeline for rate limiting to work.
app.UseRateLimiter();


// ============================================================
// CORS MIDDLEWARE
// ============================================================

// This allows the configured frontend to communicate with the API.
app.UseCors("AllowFrontend");


// ============================================================
// AUTHENTICATION
// ============================================================

// Enables authentication middleware.
app.UseAuthentication();


// ============================================================
// AUTHORIZATION
// ============================================================

// Enables authorization middleware.
app.UseAuthorization();


// ============================================================
// CONTROLLERS
// ============================================================

// Maps Controller routes to the application.
app.MapControllers();


// ============================================================
// WRONG MIDDLEWARE POSITION - EXAMPLE
// ============================================================
//using my own  middleware at the wrong place
//app.UseMiddleware<RequestLoggingMiddleware>();

// ============================================================
// RUN APPLICATION
// ============================================================
app.Run();