using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HospitalizationsController : ControllerBase
    {
        private readonly IHospitalizationService
            _hospitalizationService;

        public HospitalizationsController(
            IHospitalizationService hospitalizationService)
        {
            _hospitalizationService =
                hospitalizationService;
        }
        // Creates a new hospitalization record for the authenticated patient.
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(
            [FromBody] CreateHospitalizationDto dto)
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
                await _hospitalizationService
                    .CreateAsync(
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