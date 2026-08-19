using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [Route("api/v1/AdminDashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _service;

        public AdminDashboardController(
            IAdminDashboardService service)
        {
            _service = service;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            var overview =
                await _service.GetOverviewAsync();

            return Ok(overview);
        }
        [HttpGet("patients-at-risk")]
        public async Task<IActionResult> GetPatientsAtRisk()
        {
            var patients =
                await _service.GetPatientsAtRiskAsync();

            return Ok(patients);
        }
    }
}