using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [Route("api/v1/VitalSignsController")]
    [ApiController]
    [Authorize]
    public class VitalSignsController : ControllerBase
    {
        private readonly IVitalSignService _service;

        public VitalSignsController(IVitalSignService service)
        {
            _service = service;
        }

        // GET: api/VitalSigns/my
        // Patient: Get own vital signs
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyVitalSigns()
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

            var vitalSigns =
                await _service.GetMyVitalSignsAsync(userId);

            return Ok(vitalSigns);
        }

        // GET: api/VitalSigns
        // Admin: Get all vital signs
        [HttpGet]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var vitalSigns = await _service.GetAllAsync();

            return Ok(vitalSigns);
        }

        // GET: api/VitalSigns/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vitalSign = await _service.GetByIdAsync(id);

            if (vitalSign == null)
            {
                return NotFound(new
                {
                    message = "Vital sign not found."
                });
            }

            return Ok(vitalSign);
        }

        // GET: api/VitalSigns/patient/{patientId}
        // Admin: Get vital signs for a specific patient
        [HttpGet("patient/{patientId}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var vitalSigns =
                await _service.GetByPatientIdAsync(patientId);

            return Ok(vitalSigns);
        }

        // GET: api/VitalSigns/filter
        // Admin: Filter vital signs by patient information
        [HttpGet("filter")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(
            [FromQuery] VitalSignFilterDto filter)
        {
            var vitalSigns =
                await _service.FilterAsync(filter);

            return Ok(vitalSigns);
        }

        [HttpPost]
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
       [FromBody] CreateVitalSignDto dto)
        {
            var createdVitalSign =
                await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdVitalSign.VitalSignId },
                createdVitalSign);
        }

        // PUT: api/VitalSigns/{id}
        // Admin: Update a vital sign record
        [HttpPut("{id}")]
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateVitalSignDto dto)
        {
            var updated =
                await _service.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Vital sign not found."
                });
            }

            return Ok(new
            {
                message = "Vital sign updated successfully."
            });
        }

        // DELETE: api/VitalSigns/{id}
        // Admin: Delete a vital sign record
        [HttpDelete("{id}")]
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Vital sign not found."
                });
            }

            return Ok(new
            {
                message = "Vital sign deleted successfully."
            });
        }
        [HttpGet("my/compare-latest")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CompareLatestTwo()
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

            var comparison =
                await _service.CompareLatestTwoAsync(userId);

            if (comparison == null)
            {
                return BadRequest(new
                {
                    message =
                        "At least two vital sign records are required for comparison."
                });
            }

            return Ok(comparison);
        }
        [HttpGet("my/compare-dates")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CompareByDates(
    [FromQuery] DateTime firstDate,
    [FromQuery] DateTime secondDate)
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

            if (firstDate.Date == secondDate.Date)
            {
                return BadRequest(new
                {
                    message =
                        "The two dates must be different."
                });
            }

            var comparison =
                await _service.CompareByDatesAsync(
                    userId,
                    firstDate,
                    secondDate);

            if (comparison == null)
            {
                return NotFound(new
                {
                    message =
                        "No vital sign record was found for one or both dates."
                });
            }

            return Ok(comparison);
        }
    }
}