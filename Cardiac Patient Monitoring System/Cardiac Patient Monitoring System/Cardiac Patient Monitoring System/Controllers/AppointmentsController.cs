// This controller handles appointment management for Patients and Admins.
// It provides endpoints for viewing, filtering, creating, and updating appointments.

using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [Route("api/v1/AppointmentService")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(
            IAppointmentService service)
        {
            _service = service;
        }

        // Gets appointments belonging to the authenticated patient.
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
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

            var appointments =
                await _service.GetMyAppointmentsAsync(
                    userId);

            return Ok(appointments);
        }

        // Gets all appointments for Admin users.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var appointments =
                await _service.GetAllAsync();

            return Ok(appointments);
        }

        // Gets a specific appointment by its ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment =
                await _service.GetByIdAsync(id);

            if (appointment == null)
            {
                return NotFound(new
                {
                    message = "Appointment not found."
                });
            }

            return Ok(appointment);
        }

        // Gets all appointments belonging to a specific patient.
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetByPatientId(int patientId)
        {
            var appointments =
                await _service.GetByPatientIdAsync(
                    patientId);

            return Ok(appointments);
        }

        // Filters appointments for the authenticated patient.
        [HttpGet("filter")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult>
            FilterMyAppointments(
                [FromQuery] AppointmentFilterDto filter)
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

            var appointments =
                await _service.FilterMyAppointmentsAsync(
                    userId,
                    filter);

            return Ok(appointments);
        }

        // Filters all appointments for Admin users.
        [HttpGet("admin/filter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FilterAll(
            [FromQuery] AppointmentFilterDto filter)
        {
            var appointments =
                await _service.FilterAllAsync(filter);

            return Ok(appointments);
        }

        // Creates a new appointment for the authenticated patient.
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppointmentDto dto)
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

            var appointment =
                await _service.CreateAsync(
                    userId,
                    dto);

            if (appointment == null)
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
                    id = appointment.AppointmentId
                },
                appointment);
        }

        // Updates appointment data for the authenticated patient.
        [HttpPut("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateAppointmentDto dto)
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

            var updated =
                await _service.UpdateAsync(
                    id,
                    userId,
                    dto);

            if (!updated)
            {
                return BadRequest(new
                {
                    message =
                        "Appointment cannot be updated."
                });
            }

            return Ok(new
            {
                message =
                    "Appointment updated successfully."
            });
        }

        // Updates only the status of an appointment.
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateAppointmentStatusDto dto)
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

            var updated =
                await _service.UpdateStatusAsync(
                    id,
                    userId,
                    dto);

            if (!updated)
            {
                return BadRequest(new
                {
                    message =
                        "Appointment status cannot be updated."
                });
            }

            return Ok(new
            {
                message =
                    "Appointment status updated successfully."
            });
        }
    }
}