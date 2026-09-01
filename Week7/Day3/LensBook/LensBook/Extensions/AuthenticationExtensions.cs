using System.Security.Claims;
using System.Text;

using LensBook.Configuration;
using LensBook.DATA;
using LensBook.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

// this file is used to configure authentication and authorization services for the application.
// It sets up JWT authentication, configures Identity options,
// and adds necessary services to the dependency injection container.
namespace LensBook.Extensions
{
    public static class AuthenticationExtensions
    {
       
        // Authentication Configuration
      

        public static IServiceCollection AddApplicationAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
           
            // JWT Settings
            
            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));


          
            // Identity
          
            services
                .AddIdentity<ApplicationUser, IdentityRole<int>>(
                    options =>
                    {
                        options.Password.RequiredLength = 8;
                        options.Password.RequireDigit = true;
                        options.Password.RequireUppercase = true;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireNonAlphanumeric = true;

                        options.User.RequireUniqueEmail = true;
                    })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


         
            // Get JWT Settings
       
            var jwtSettings =
                configuration
                    .GetSection("JwtSettings")
                    .Get<JwtSettings>()
                ?? throw new InvalidOperationException(
                    "JwtSettings are not configured.");


           
            // Authentication
        
            services
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
                                    jwtSettings.Issuer,

                                ValidAudience =
                                    jwtSettings.Audience,

                                IssuerSigningKey =
                                    new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(
                                            jwtSettings.SecretKey)),

                                ClockSkew =
                                    TimeSpan.Zero,
                                    RoleClaimType = ClaimTypes.Role
                            };
                    });


           
            // Authorization
          
            services.AddAuthorization();


            return services;
        }
    }
}