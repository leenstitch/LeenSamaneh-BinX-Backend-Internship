using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LabResultsController : ControllerBase
    {
        private readonly ILabResultService _service;

        public LabResultsController(
            ILabResultService service)
        {
            _service = service;
        }
        // Creates a new LabResults record for the authenticated patient.
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(
            [FromBody] CreateLabResultDto dto)
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
                    message =
                        "Invalid user identity."
                });
            }

            var result =
                await _service.CreateAsync(
                    userId,
                    dto);

            if (result == null)
            {
                return NotFound(new
                {
                    message =
                        "Patient profile not found."
                });
            }

            return Ok(result);
        }
    }
}