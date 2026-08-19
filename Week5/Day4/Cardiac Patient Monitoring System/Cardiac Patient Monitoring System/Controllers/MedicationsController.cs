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

        // GET: api/Medications/my
        // Patient: Get own medications
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

        // GET: api/Medications
        // Admin: Get all medications
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var medications =
                await _service.GetAllAsync();

            return Ok(medications);
        }

        // GET: api/Medications/{id}
        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin")]
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

        // GET: api/Medications/patient/{patientId}
        // Admin: Get medications of a specific patient
        [HttpGet("patient/{patientId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetByPatientId(int patientId)
        {
            var medications =
                await _service.GetByPatientIdAsync(patientId);

            return Ok(medications);
        }

        // POST: api/Medications
        // Admin: Create medication
        [HttpPost]
       // [Authorize(Roles = "Admin")]
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

        // PUT: api/Medications/{id}
        // Admin: Update medication
        [HttpPut("{id}")]
      //  [Authorize(Roles = "Admin")]
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

        // DELETE: api/Medications/{id}
        // Admin: Delete medication
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
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