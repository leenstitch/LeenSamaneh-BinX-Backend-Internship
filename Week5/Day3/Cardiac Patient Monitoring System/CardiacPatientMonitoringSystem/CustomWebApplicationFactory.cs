using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CardiacPatientMonitoringSystem
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the original DbContext configuration
                // that uses SQL Server.
                var dbContextOptionsConfiguration =
                    services.SingleOrDefault(
                        service =>
                            service.ServiceType ==
                            typeof(
                                IDbContextOptionsConfiguration
                                <ApplicationDbContext>));

                if (dbContextOptionsConfiguration != null)
                {
                    services.Remove(
                        dbContextOptionsConfiguration);
                }

                // Remove the existing DbContext registrations.
                var dbContextOptions =
                    services.SingleOrDefault(
                        service =>
                            service.ServiceType ==
                            typeof(
                                DbContextOptions
                                <ApplicationDbContext>));

                if (dbContextOptions != null)
                {
                    services.Remove(dbContextOptions);
                }

                // Register an isolated In-Memory database
                // for integration tests.
                services.AddDbContext<ApplicationDbContext>(
                    options =>
                    {
                        options.UseInMemoryDatabase(
                            "CardiacPatientMonitoringTestDb");
                    });

                // Replace the real authentication with
                // test authentication.
                services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme =
                            "TestAuthentication";

                        options.DefaultChallengeScheme =
                            "TestAuthentication";
                    })
                    .AddScheme<
                        AuthenticationSchemeOptions,
                        TestAuthenticationHandler>(
                        "TestAuthentication",
                        options =>
                        {
                        });

                // Create the test database.
                var serviceProvider =
                    services.BuildServiceProvider();

                using var scope =
                    serviceProvider.CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureCreated();

                // Add test data.
                SeedTestData(db);
            });
        }

        private static void SeedTestData(
            ApplicationDbContext db)
        {
            // Create the test user.
            var user = new ApplicationUser
            {
                Id = 1,
                UserName = "test@example.com",
                NormalizedUserName = "TEST@EXAMPLE.COM",
                Email = "test@example.com",
                NormalizedEmail = "TEST@EXAMPLE.COM",
                EmailConfirmed = true
            };

            db.Users.Add(user);

            // Create the patient linked to the test user.
            var patient = new Patient
            {
                PatientId = 1,
                UserId = 1,
                FirstName = "Test",
                LastName = "Patient",
                DateOfBirth = new DateTime(1995, 1, 1),
                PatientGender = Patient.Gender.Male,
                PrimaryPhone = "0599999999",
                NationalId = "TEST123456",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var vitalSign = new VitalSign
            {
                VitalSignId = 1,
                Patient = patient,
                PatientId = patient.PatientId,
                HeartRate = 80,
                SystolicPressure = 120,
                DiastolicPressure = 80,
                OxygenSaturation = 98,
                Temperature = 36.7m,
                MeasuredAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                Notes = "Integration test vital sign"
            };

            
            db.Patients.Add(patient);
            db.VitalSigns.Add(vitalSign);

            db.SaveChanges();
        }
    }
}