// This controller handles medication management for Patients and Admins.
// It provides endpoints for retrieving, creating, updating, deleting,
// and filtering medications.

using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [Route("api/v1/MedicationsController")]
    [ApiController]
    [Authorize]
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationService _service;

        public MedicationsController(
            IMedicationService service)
        {
            _service = service;
        }

        // Gets medications belonging to the authenticated patient.
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyMedications()
        {
            var userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message =
                        "User ID was not found in the token."
                });
            }

            var medications =
                await _service.GetMyMedicationsAsync(userId);

            return Ok(medications);
        }

        // Gets all medications.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var medications =
                await _service.GetAllAsync();

            return Ok(medications);
        }

        // Gets a specific medication by its ID.
        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetById(int id)
        {
            var medication =
                await _service.GetByIdAsync(id);

            if (medication == null)
            {
                return NotFound(new
                {
                    message = "Medication not found."
                });
            }

            return Ok(medication);
        }

        // Gets medications belonging to a specific patient.
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetByPatientId(int patientId)
        {
            var medications =
                await _service.GetByPatientIdAsync(patientId);

            return Ok(medications);
        }

        // Creates a medication for the patient linked to the authenticated user.
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(
            CreateMedicationDto dto)
        {
            var userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var medication =
                await _service.CreateAsync(
                    userId,
                    dto);

            if (medication == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found."
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = medication.MedicationId },
                medication);
        }

        // Updates an existing medication.
        [HttpPut("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Update(
            int id,
            UpdateMedicationDto dto)
        {
            var updated =
                await _service.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Medication not found."
                });
            }

            return Ok(new
            {
                message =
                    "Medication updated successfully."
            });
        }

        // Deletes an existing medication.
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Medication not found."
                });
            }

            return Ok(new
            {
                message =
                    "Medication deleted successfully."
            });
        }

        // Filters medications belonging to the authenticated patient.
        [HttpGet("my/filter")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> FilterMyMedications(
            [FromQuery] MedicationFilterDto filter)
        {
            var userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(
                userIdClaim,
                out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var medications =
                await _service.FilterMyMedicationsAsync(
                    userId,
                    filter);

            return Ok(medications);
        }
    }
}