using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicalProcedureDto_s;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicalProceduresController : ControllerBase
    {
        private readonly IMedicalProcedureService
            _medicalProcedureService;

        public MedicalProceduresController(
            IMedicalProcedureService medicalProcedureService)
        {
            _medicalProcedureService =
                medicalProcedureService;
        }


        // Creates a new  MedicalProcedures record for the authenticated patient.

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(
            [FromBody] CreateMedicalProcedureDto dto)
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
                await _medicalProcedureService
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