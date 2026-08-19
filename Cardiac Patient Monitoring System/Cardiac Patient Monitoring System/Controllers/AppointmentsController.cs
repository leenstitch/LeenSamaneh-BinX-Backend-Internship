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

        // ============================================
        // GET MY APPOINTMENTS
        // GET: api/Appointments/my
        // Patient
        // ============================================

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

        // ============================================
        // GET ALL APPOINTMENTS
        // GET: api/Appointments
        // Admin
        // ============================================

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var appointments =
                await _service.GetAllAsync();

            return Ok(appointments);
        }
        // ============================================
        // GET APPOINTMENT BY ID
        // GET: api/Appointments/{id}
        // ============================================

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

        // ============================================
        // GET PATIENT APPOINTMENTS
        // GET: api/Appointments/patient/{patientId}
        // Admin
        // ============================================

        [HttpGet("patient/{patientId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetByPatientId(int patientId)
        {
            var appointments =
                await _service.GetByPatientIdAsync(
                    patientId);

            return Ok(appointments);
        }

        // ============================================
        // FILTER MY APPOINTMENTS
        // GET: api/Appointments/filter
        // Patient
        // ============================================

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

        // ============================================
        // FILTER ALL APPOINTMENTS
        // GET: api/Appointments/admin/filter
        // Admin
        // ============================================

        [HttpGet("admin/filter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FilterAll(
            [FromQuery] AppointmentFilterDto filter)
        {
            var appointments =
                await _service.FilterAllAsync(filter);

            return Ok(appointments);
        }

        // ============================================
        // CREATE APPOINTMENT
        // POST: api/Appointments
        // Patient
        // ============================================

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

        // ============================================
        // UPDATE APPOINTMENT DATA
        // PUT: api/Appointments/{id}
        // Patient
        // ============================================

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

        // ============================================
        // UPDATE APPOINTMENT STATUS ONLY
        // PUT: api/Appointments/{id}/status
        // Patient
        // ============================================

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