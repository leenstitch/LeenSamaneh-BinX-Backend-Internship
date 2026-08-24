// This service handles the business operations for the Admin Dashboard.
// It retrieves dashboard overview data and patients who are at risk
// through the repository layer.

using Cardiac_Patient_Monitoring_System.DTO_S.AdminDashboardDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class AdminDashboardService
        : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _repository;

        public AdminDashboardService(
            IAdminDashboardRepository repository)
        {
            _repository = repository;
        }

        // Returns the Admin Dashboard overview data.
        public async Task<AdminDashboardOverviewDto>
            GetOverviewAsync()
        {
            return await _repository.GetOverviewAsync();
        }

        // Returns patients who are currently at risk.
        public async Task<IEnumerable<PatientAtRiskDto>>
            GetPatientsAtRiskAsync()
        {
            return await _repository.GetPatientsAtRiskAsync();
        }
    }
}