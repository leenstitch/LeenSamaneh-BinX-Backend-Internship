using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [Route("api/v1/DiagnosesController")]
    [ApiController]
    [Authorize]
    public class DiagnosesController : ControllerBase
    {
        private readonly IDiagnosisService _service;

        public DiagnosesController(IDiagnosisService service)
        {
            _service = service;
        }

        // GET: api/Diagnoses/my
        // Patient: Get own diagnoses
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyDiagnoses()
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var diagnoses =
                await _service.GetMyDiagnosesAsync(userId);

            return Ok(diagnoses);
        }

        // GET: api/Diagnoses
        // Admin: Get all diagnoses
        [HttpGet]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var diagnoses =
                await _service.GetAllAsync();

            return Ok(diagnoses);
        }

        // GET: api/Diagnoses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var diagnosis =
                await _service.GetByIdAsync(id);

            if (diagnosis == null)
            {
                return NotFound(new
                {
                    message = "Diagnosis not found."
                });
            }

            return Ok(diagnosis);
        }

        // GET: api/Diagnoses/patient/{patientId}
        // Admin: Get diagnoses for a specific patient
        [HttpGet("patient/{patientId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByPatientId(
            int patientId)
        {
            var diagnoses =
                await _service.GetByPatientIdAsync(patientId);

            return Ok(diagnoses);
        }

        // GET: api/Diagnoses/filter
        // Admin: Filter diagnoses
        [HttpGet("filter")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(
            [FromQuery] DiagnosisFilterDto filter)
        {
            var diagnoses =
                await _service.FilterAsync(filter);

            return Ok(diagnoses);
        }

        // POST: api/Diagnoses
        // Admin: Create diagnosis
        [HttpPost]
        public async Task<IActionResult> Create(
    [FromBody] CreateDiagnosisDto dto)
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var createdDiagnosis =
                await _service.CreateAsync(userId, dto);

            if (createdDiagnosis == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found."
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdDiagnosis.DiagnosisId },
                createdDiagnosis);
        }

        // PUT: api/Diagnoses/{id}
        // Admin: Update diagnosis
        [HttpPut("{id}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDiagnosisDto dto)
        {
            var updated =
                await _service.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Diagnosis not found."
                });
            }

            return Ok(new
            {
                message = "Diagnosis updated successfully."
            });
        }

        // DELETE: api/Diagnoses/{id}
        // Admin: Delete diagnosis
        [HttpDelete("{id}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Diagnosis not found."
                });
            }

            return Ok(new
            {
                message = "Diagnosis deleted successfully."
            });
        }
    }
}