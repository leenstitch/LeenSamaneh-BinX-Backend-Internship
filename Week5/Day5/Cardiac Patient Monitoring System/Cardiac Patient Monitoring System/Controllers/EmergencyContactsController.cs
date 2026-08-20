using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.EmergencyContactDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [Route("api/v1/EmergencyContactsController")]
    [ApiController]
    [Authorize]
    public class EmergencyContactsController
        : ControllerBase
    {
        private readonly IEmergencyContactService _service;

        public EmergencyContactsController(
            IEmergencyContactService service)
        {
            _service = service;
        }

        // Patient: Get own emergency contacts
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult>
            GetMyEmergencyContacts()
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
                    message =
                        "Invalid user identity."
                });
            }

            var contacts =
                await _service
                    .GetMyEmergencyContactsAsync(
                        userId);

            return Ok(contacts);
        }

        // Admin: Get all emergency contacts
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var contacts =
                await _service.GetAllAsync();

            return Ok(contacts);
        }

        // Admin: Get emergency contact by id
        [HttpGet("{id}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact =
                await _service.GetByIdAsync(id);

            if (contact == null)
            {
                return NotFound(new
                {
                    message =
                        "Emergency contact not found."
                });
            }

            return Ok(contact);
        }

        // Admin: Get contacts for a specific patient
        [HttpGet("patient/{patientId}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetByPatientId(int patientId)
        {
            var contacts =
                await _service.GetByPatientIdAsync(
                    patientId);

            return Ok(contacts);
        }

        // Create contact for the logged-in patient
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(
            [FromBody] CreateEmergencyContactDto dto)
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
                    message =
                        "Invalid user identity."
                });
            }

            var createdContact =
                await _service.CreateAsync(
                    userId,
                    dto);

            if (createdContact == null)
            {
                return NotFound(new
                {
                    message =
                        "Patient profile not found."
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id =
                        createdContact.EmergencyContactId
                },
                createdContact);
        }

        // Admin: Update emergency contact
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateEmergencyContactDto dto)
        {
            var updated =
                await _service.UpdateAsync(
                    id,
                    dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        "Emergency contact not found."
                });
            }

            return Ok(new
            {
                message =
                    "Emergency contact updated successfully."
            });
        }

        // Admin: Delete emergency contact
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
                    message =
                        "Emergency contact not found."
                });
            }

            return Ok(new
            {
                message =
                    "Emergency contact deleted successfully."
            });
        }
    }
}