using System.Security.Claims;
using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s;
using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEventAnalysisDto_s;
using Cardiac_Patient_Monitoring_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CardiacEventAnalysisController : ControllerBase
    {
        private readonly ICardiacEventAnalysisService
            _cardiacEventAnalysisService;


        public CardiacEventAnalysisController(
            ICardiacEventAnalysisService cardiacEventAnalysisService)
        {
            _cardiacEventAnalysisService =
                cardiacEventAnalysisService;
        }


       
        // Analyzes a cardiac event using the patient's medical data
        // from a specified number of days before the event.
        [HttpGet("{cardiacEventId}")]
        public async Task<IActionResult> AnalyzeEvent(
            int cardiacEventId,
            [FromQuery] int daysBefore = 14)
        {

            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user identity.");
            }


            var result =
                await _cardiacEventAnalysisService
                    .AnalyzeEventAsync(
                        userId,
                        cardiacEventId,
                        daysBefore);


            if (result == null)
            {
                return NotFound(
                    "Cardiac event was not found or does not belong to the authenticated patient.");
            }


           

            return Ok(result);
        }

       
        // Creates a new cardiac event for the authenticated patient.
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCardiacEventDto dto)
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
                await _cardiacEventAnalysisService.CreateAsync(
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
    ////    // Returns the latest vital sign recorded before a cardiac event.
    ////    [HttpGet("{cardiacEventId}/latest-vital")]
    ////    public async Task<IActionResult>
    ////GetLatestVitalBeforeEvent(
    ////    int cardiacEventId)
    ////    {
    ////        var userId = int.Parse(
    ////            User.FindFirst("sub")!.Value);

    ////        var result =
    ////            await _cardiacEventAnalysisService
    ////                .GetLatestVitalBeforeEventAsync(
    ////                    userId,
    ////                    cardiacEventId);

    ////        if (result == null)
    ////        {
    ////            return NotFound(
    ////                "No vital sign found before this cardiac event.");
    ////        }

    ////        return Ok(result);
    ////    }
    }
}