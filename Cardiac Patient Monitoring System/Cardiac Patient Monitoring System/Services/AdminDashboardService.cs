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

        public async Task<AdminDashboardOverviewDto>
            GetOverviewAsync()
        {
            return await _repository.GetOverviewAsync();
        }
        public async Task<IEnumerable<PatientAtRiskDto>>
    GetPatientsAtRiskAsync()
        {
            return await _repository.GetPatientsAtRiskAsync();
        }
    }
}