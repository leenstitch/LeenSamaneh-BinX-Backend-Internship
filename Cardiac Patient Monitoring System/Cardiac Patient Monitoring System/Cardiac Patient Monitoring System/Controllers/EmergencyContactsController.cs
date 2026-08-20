// This controller handles emergency contact management.
// It provides endpoints for patients to manage their own contacts
// and for administrators to view, update, and delete contacts.

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

        // Gets emergency contacts belonging to the authenticated patient.
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

        // Gets all emergency contacts.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var contacts =
                await _service.GetAllAsync();

            return Ok(contacts);
        }

        // Gets a specific emergency contact by ID.
        [HttpGet("{id}")]
        [Authorize(Roles = "Patient")]
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

        // Gets emergency contacts belonging to a specific patient.
        [HttpGet("patient/{patientId}")]
         [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetByPatientId(int patientId)
        {
            var contacts =
                await _service.GetByPatientIdAsync(
                    patientId);

            return Ok(contacts);
        }

        // Creates an emergency contact for the authenticated patient.
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

        // Updates an existing emergency contact.
        [HttpPut("{id}")]
        [Authorize(Roles = "Patient")]
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

        // Deletes an existing emergency contact.
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