using System;
using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CardiacPatientMonitoringSystem.Integration
{
    // ================================================================
    // Custom Web Application Factory
    // ================================================================
    //
    // Configures the real ASP.NET Core application for integration tests.
    //
    // The production SQL Server database is replaced with an isolated
    // In-Memory database.
    //
    // Real authentication is replaced with TestAuthenticationHandler.
    //
    // The test database contains:
    //
    // User 1 -> Patient 1 -> Abnormal Vital Sign
    // User 2 -> Patient 2 -> No Vital Signs
    //
    // ================================================================

    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // =====================================================
                // Remove Production Database
                // =====================================================

                services.RemoveAll<
                    DbContextOptions<ApplicationDbContext>>();

                services.RemoveAll<
                    IDbContextOptionsConfiguration
                        <ApplicationDbContext>>();

                // =====================================================
                // Add Test In-Memory Database
                // =====================================================

                var databaseName =
                    $"CardiacIntegrationTestDb_{Guid.NewGuid()}";

                services.AddDbContext<ApplicationDbContext>(
                    options =>
                    {
                        options.UseInMemoryDatabase(
                            databaseName);
                    });

                // =====================================================
                // Test Authentication
                // =====================================================

                services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme =
                            "TestAuthentication";

                        options.DefaultChallengeScheme =
                            "TestAuthentication";

                        options.DefaultScheme =
                            "TestAuthentication";
                    })
                    .AddScheme<
                        AuthenticationSchemeOptions,
                        TestAuthenticationHandler>(
                        "TestAuthentication",
                        _ =>
                        {
                        });

                // =====================================================
                // Build Test Service Provider
                // =====================================================

                var serviceProvider =
                    services.BuildServiceProvider();

                using var scope =
                    serviceProvider.CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<
                            ApplicationDbContext>();

                db.Database.EnsureCreated();

                // =====================================================
                // Seed Integration Test Data
                // =====================================================

                SeedTestData(db);
            });
        }

        private static void SeedTestData(
            ApplicationDbContext db)
        {
            // =====================================================
            // USER 1
            // Patient with abnormal vital signs
            // =====================================================

            var user1 =
                new ApplicationUser
                {
                    Id = 1,

                    UserName =
                        "patient1@test.com",

                    NormalizedUserName =
                        "PATIENT1@TEST.COM",

                    Email =
                        "patient1@test.com",

                    NormalizedEmail =
                        "PATIENT1@TEST.COM",

                    EmailConfirmed = true,

                    SecurityStamp =
                        Guid.NewGuid().ToString(),

                    ConcurrencyStamp =
                        Guid.NewGuid().ToString()
                };

            db.Users.Add(user1);

            // =====================================================
            // PATIENT 1
            // =====================================================

            var patient1 =
                new Patient
                {
                    PatientId = 1,

                    UserId = 1,

                    FirstName =
                        "Test",

                    LastName =
                        "Patient One",

                    DateOfBirth =
                        new DateTime(
                            1995,
                            1,
                            1),

                    PatientGender =
                        Patient.Gender.Male,

                    PrimaryPhone =
                        "0599999999",

                    NationalId =
                        "TEST111111",

                    CreatedAt =
                        DateTime.UtcNow,

                    UpdatedAt =
                        DateTime.UtcNow
                };

            db.Patients.Add(patient1);

            // =====================================================
            // ABNORMAL VITAL SIGN FOR PATIENT 1
            // =====================================================

            var abnormalVitalSign =
                new VitalSign
                {
                    VitalSignId = 1,

                    PatientId = 1,

                    HeartRate = 120,

                    SystolicPressure = 150,

                    DiastolicPressure = 95,

                    OxygenSaturation = 88,

                    Temperature = 38.5m,

                    MeasuredAt =
                        DateTime.UtcNow,

                    CreatedAt =
                        DateTime.UtcNow,

                    Notes =
                        "Abnormal integration test reading."
                };

            db.VitalSigns.Add(
                abnormalVitalSign);

            // =====================================================
            // USER 2
            // Patient WITHOUT vital signs
            // =====================================================

            var user2 =
                new ApplicationUser
                {
                    Id = 2,

                    UserName =
                        "patient2@test.com",

                    NormalizedUserName =
                        "PATIENT2@TEST.COM",

                    Email =
                        "patient2@test.com",

                    NormalizedEmail =
                        "PATIENT2@TEST.COM",

                    EmailConfirmed = true,

                    SecurityStamp =
                        Guid.NewGuid().ToString(),

                    ConcurrencyStamp =
                        Guid.NewGuid().ToString()
                };

            db.Users.Add(user2);

            // =====================================================
            // PATIENT 2
            // No Vital Signs
            // =====================================================

            var patient2 =
                new Patient
                {
                    PatientId = 2,

                    UserId = 2,

                    FirstName =
                        "Test",

                    LastName =
                        "Patient Two",

                    DateOfBirth =
                        new DateTime(
                            1998,
                            5,
                            10),

                    PatientGender =
                        Patient.Gender.Female,

                    PrimaryPhone =
                        "0598888888",

                    NationalId =
                        "TEST222222",

                    CreatedAt =
                        DateTime.UtcNow,

                    UpdatedAt =
                        DateTime.UtcNow
                };

            db.Patients.Add(patient2);

            // =====================================================
            // Save Test Data
            // =====================================================

            db.SaveChanges();
        }
    }
}