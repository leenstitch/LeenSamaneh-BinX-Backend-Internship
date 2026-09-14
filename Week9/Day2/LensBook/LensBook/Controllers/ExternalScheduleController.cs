using LensBook.Dto_s.ExternalScheduleDto_s;
using LensBook.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LensBook.Controllers
{
    [Route("api/v1/ExternalScheduleController")]
    [ApiController]
    [Authorize(Roles = "Photographer")]
    public class ExternalScheduleController
        : ControllerBase
    {
        private readonly IExternalScheduleService
            _externalScheduleService;

        public ExternalScheduleController(
            IExternalScheduleService externalScheduleService)
        {
            _externalScheduleService =
                externalScheduleService;
        }

        [HttpPost]
        public async Task<IActionResult>
            AddExternalSchedule(
                CreateExternalScheduleDto dto)
        {
            var result =
                await _externalScheduleService
                    .AddExternalScheduleAsync(dto);

            return Ok(result);
        }
    }
}