// This interface defines the data access operations for the Admin Dashboard.
// It retrieves dashboard overview information and patients who are at risk.

using Cardiac_Patient_Monitoring_System.DTO_S.AdminDashboardDto_s;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IAdminDashboardRepository
    {
        // Returns the overview data for the Admin Dashboard.
        Task<AdminDashboardOverviewDto> GetOverviewAsync();

        // Returns patients who are currently at risk.
        Task<IEnumerable<PatientAtRiskDto>> GetPatientsAtRiskAsync();
    }
}