// This extension class configures authentication and authorization
// for the application using ASP.NET Core Identity, JWT authentication,
// and custom permission-based authorization policies.

using System.Text;
using Cardiac_Patient_Monitoring_System.Configuration;
using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Cardiac_Patient_Monitoring_System.Extensions
{
    public static class AuthenticationExtensions
    {
        // Configures Identity, JWT authentication, and authorization policies.
        public static IServiceCollection AddApplicationAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // JWT Settings
            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));

            // Identity
            services
                .AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
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
            var jwtSettings = configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>()
                ?? throw new InvalidOperationException(
                    "JwtSettings are not configured.");

            // Authentication
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
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

                            ClockSkew = TimeSpan.Zero
                        };
                });

            // Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "PatientRead",
                    policy =>
                        policy.RequireClaim(
                            "permission",
                            "Patient.Read"));

                options.AddPolicy(
                    "PatientCreate",
                    policy =>
                        policy.RequireClaim(
                            "permission",
                            "Patient.Create"));

                options.AddPolicy(
                    "DiagnosisCreate",
                    policy =>
                        policy.RequireClaim(
                            "permission",
                            "Diagnosis.Create"));

                options.AddPolicy(
                    "MedicationCreate",
                    policy =>
                        policy.RequireClaim(
                            "permission",
                            "Medication.Create"));

                options.AddPolicy(
                    "AppointmentCreate",
                    policy =>
                        policy.RequireClaim(
                            "permission",
                            "Appointment.Create"));
            });

            return services;
        }
    }
}