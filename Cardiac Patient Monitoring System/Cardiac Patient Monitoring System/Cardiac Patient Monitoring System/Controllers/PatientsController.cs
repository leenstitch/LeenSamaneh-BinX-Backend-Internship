// This controller handles patient profile management and health monitoring.
// It provides endpoints for patient profiles, health summaries, and health status.

using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/v1/PatientsController")]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // Gets the authenticated patient's own profile.
        [HttpGet("me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdValue =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var patient =
                await _patientService.GetMyProfileAsync(
                    userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found."
                });
            }

            return Ok(patient);
        }

        // Updates the authenticated patient's own profile.
        [HttpPut("me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateMyProfile(
            UpdatePatientDto dto)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var patient =
                await _patientService.UpdateMyProfileAsync(
                    userId,
                    dto);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found."
                });
            }

            return Ok(patient);
        }

        // Gets all patients.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients =
                await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }

        // Deletes a patient by ID.
        [HttpDelete("{patientId}")]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(
            int patientId)
        {
            var deleted =
                await _patientService.DeletePatientAsync(
                    patientId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Patient not found."
                });
            }

            return Ok(new
            {
                message = "Patient deleted successfully."
            });
        }

        // Gets the authenticated patient's health summary.
        [HttpGet("me/health-summary")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyHealthSummary()
        {
            var userIdValue =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var summary =
                await _patientService
                    .GetMyHealthSummaryAsync(userId);

            if (summary == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found."
                });
            }

            return Ok(summary);
        }

        // Gets the health summary of a specific patient for Admin users.
        [HttpGet("{patientId}/health-summary")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetHealthSummary(int patientId)
        {
            var summary =
                await _patientService
                    .GetHealthSummaryAsync(patientId);

            if (summary == null)
            {
                return NotFound(new
                {
                    message = "Patient not found."
                });
            }

            return Ok(summary);
        }

        // Gets the authenticated patient's current health status.
        [HttpGet("me/health-status")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyHealthStatus()
        {
            var userIdValue =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(
                userIdValue,
                out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var result =
                await _patientService
                    .GetMyHealthStatusAsync(userId);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found."
                });
            }

            return Ok(result);
        }

        // Gets the health status of a specific patient for Admin users.
        [HttpGet("{patientId}/health-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetHealthStatus(int patientId)
        {
            var result =
                await _patientService
                    .GetHealthStatusAsync(patientId);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Patient not found."
                });
            }

            return Ok(result);
        }
    }
}