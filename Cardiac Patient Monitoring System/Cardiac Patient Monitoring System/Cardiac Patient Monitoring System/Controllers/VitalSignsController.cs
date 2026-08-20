// This controller handles vital-sign management and monitoring.
// It provides endpoints for retrieving, creating, updating, deleting,
// filtering, and comparing patient vital-sign records.

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

        // Gets vital signs belonging to the authenticated patient.
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

        // Gets all vital-sign records.
        [HttpGet]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var vitalSigns = await _service.GetAllAsync();

            return Ok(vitalSigns);
        }

        // Gets a specific vital-sign record by ID.
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

        // Gets vital-sign records belonging to a specific patient.
        [HttpGet("patient/{patientId}")]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var vitalSigns =
                await _service.GetByPatientIdAsync(patientId);

            return Ok(vitalSigns);
        }

        // Filters vital-sign records using patient information.
        [HttpGet("filter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(
            [FromQuery] VitalSignFilterDto filter)
        {
            var vitalSigns =
                await _service.FilterAsync(filter);

            return Ok(vitalSigns);
        }

        // Creates a vital-sign record for the authenticated patient.
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(
            [FromBody] CreateVitalSignDto dto)
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

            var createdVitalSign =
                await _service.CreateAsync(
                    userId,
                    dto);

            if (createdVitalSign == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found."
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = createdVitalSign.VitalSignId
                },
                createdVitalSign);
        }

        // Updates an existing vital-sign record.
        [HttpPut("{id}")]
        [Authorize(Roles = "Patient")]
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

        // Deletes an existing vital-sign record.
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
                    message = "Vital sign not found."
                });
            }

            return Ok(new
            {
                message = "Vital sign deleted successfully."
            });
        }

        // Compares the patient's two latest vital-sign records.
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

        // Compares vital-sign records from two selected dates.
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

            // Ensures that the two comparison dates are different.
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