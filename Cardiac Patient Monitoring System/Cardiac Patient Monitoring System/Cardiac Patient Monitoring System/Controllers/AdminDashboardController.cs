// ================================================================
// AdminDashboardController
// ================================================================
//
// This controller provides administrative dashboard endpoints for
// the Cardiac Patient Monitoring System.
//
//
// Available endpoints:
// - GET /api/v1/AdminDashboard/overview
//   Returns an overview of the system/dashboard.
//
// - GET /api/v1/AdminDashboard/patients-at-risk
//   Returns patients who currently require medical attention.
//
//
// ================================================================

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
        // Service responsible for retrieving and preparing
        // administrative dashboard data.
        private readonly IAdminDashboardService _service;

        // Injects the admin dashboard service through
        // dependency injection.
        public AdminDashboardController(
            IAdminDashboardService service)
        {
            _service = service;
        }

        // =========================================================
        // Get Dashboard Overview
        //
        // Returns general statistics and information needed for
        // the administrator's dashboard.
        // =========================================================

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            var overview =
                await _service.GetOverviewAsync();

            return Ok(overview);
        }

        // =========================================================
        // Get Patients At Risk
        //
        // Returns patients whose health data indicates that they
        // may require medical attention.
        // =========================================================

        [HttpGet("patients-at-risk")]
        public async Task<IActionResult> GetPatientsAtRisk()
        {
            var patients =
                await _service.GetPatientsAtRiskAsync();

            return Ok(patients);
        }
    }
}