using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Services;

namespace Cardiac_Patient_Monitoring_System.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Extension method for IServiceCollection
        // It allows us to register our custom services in Program.cs easily.
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPatientService,PatientService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            return services;
        }
    }
}