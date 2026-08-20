using Cardiac_Patient_Monitoring_System.DTO_S.AdminDashboardDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardOverviewDto>
            GetOverviewAsync();
        Task<IEnumerable<PatientAtRiskDto>>
    GetPatientsAtRiskAsync();
    }
}