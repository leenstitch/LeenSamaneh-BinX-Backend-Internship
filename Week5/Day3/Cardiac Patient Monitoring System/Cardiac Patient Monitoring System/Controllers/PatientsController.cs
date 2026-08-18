using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // Patient: Get own profile
        [HttpGet("me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var patient =
                await _patientService.GetMyProfileAsync(userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found."
                });
            }

            return Ok(patient);
        }

        // Patient: Update own profile
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

        // Admin: Get all patients
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients =
                await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }

        // Admin: Delete patient
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
        [HttpGet("{patientId:int}")]
        public async Task<IActionResult> GetPatientById(
          int patientId)
        {
            var patient =
                await _patientService.GetPatientByIdAsync(
                    patientId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient not found."
                });
            }

            return Ok(patient);
        }
    }
}