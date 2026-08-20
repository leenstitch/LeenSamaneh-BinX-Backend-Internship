using Cardiac_Patient_Monitoring_System.DTO_S.AdminDashboardDto_s;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IAdminDashboardRepository
    {
        Task<AdminDashboardOverviewDto> GetOverviewAsync();
        Task<IEnumerable<PatientAtRiskDto>> GetPatientsAtRiskAsync();
    }
}