// This interface defines the operations provided by the Admin Dashboard service.
// It is used to retrieve dashboard statistics and identify patients at risk.

using Cardiac_Patient_Monitoring_System.DTO_S.AdminDashboardDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IAdminDashboardService
    {
        // Returns the main overview information for the Admin Dashboard.
        Task<AdminDashboardOverviewDto> GetOverviewAsync();

        // Returns a list of patients who are currently at risk.
        Task<IEnumerable<PatientAtRiskDto>>
            GetPatientsAtRiskAsync();
    }
}