// This extension class registers the application's custom services
// and repositories in the dependency injection container.

using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Repositories;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Cardiac_Patient_Monitoring_System.Repositories.Repository;
using Cardiac_Patient_Monitoring_System.Services;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Registers application services and repositories using
        // the Scoped lifetime for dependency injection.
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IVitalSignRepository, VitalSignRepository>();
            services.AddScoped<IVitalSignService, VitalSignService>();
            services.AddScoped<IMedicationRepository, MedicationRepository>();
            services.AddScoped<IMedicationService, MedicationService>();
            services.AddScoped<IDiagnosisRepository, DiagnosisRepository>();
            services.AddScoped<IDiagnosisService, DiagnosisService>();
            services.AddScoped<IEmergencyContactRepository, EmergencyContactRepository>();
            services.AddScoped<IEmergencyContactService, EmergencyContactService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ICardiacEventRepository, CardiacEventRepository>();
            services.AddScoped<ICardiacEventAnalysisService, CardiacEventAnalysisService>();
            services.AddScoped<ILabResultRepository, LabResultRepository>();
            services.AddScoped<ILabResultService, LabResultService>();
            services.AddScoped<IHospitalizationRepository, HospitalizationRepository>();
            services.AddScoped<IHospitalizationService, HospitalizationService>();
            services.AddScoped<IMedicalProcedureRepository,MedicalProcedureRepository>();
            services.AddScoped<IMedicalProcedureService, MedicalProcedureService>();
            services.AddScoped< IAdminDashboardRepository,AdminDashboardRepository>();
            services.AddScoped<IAllergyRepository, AllergyRepository>();
            services.AddScoped< IAdminDashboardService,AdminDashboardService>();
            services.AddScoped<IFamilyMedicalHistoryRepository, FamilyMedicalHistoryRepository>();
            services.AddScoped<IEmergencyMedicalInformationRepository, EmergencyMedicalInformationRepository>();
            services.AddScoped<IMedicalTimelineService, MedicalTimelineService>();
            return services;
        }
    }
}