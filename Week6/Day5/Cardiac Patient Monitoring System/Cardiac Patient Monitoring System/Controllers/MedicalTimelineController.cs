using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/v1/MedicalTimelineController")]
   // [Authorize(Roles = "Admin")]
    public class MedicalTimelineController : ControllerBase
    {
        private readonly IMedicalTimelineService _medicalTimelineService;

        public MedicalTimelineController(
            IMedicalTimelineService medicalTimelineService)
        {
            _medicalTimelineService = medicalTimelineService;
        }


        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientMedicalTimeline(
            int patientId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _medicalTimelineService
                    .GetPatientMedicalTimelineAsync(
                        patientId,
                        page,
                        pageSize);

            return Ok(result);
        }
    }
}