using System.Security.Claims;
using LensBook.Dto_s.PhotographerDto_s;
using LensBook.Services;
using LensBook.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LensBook.Controllers
{
    [ApiController]
    [Route("api/v1/PhotographersController")]
    public class PhotographersController : ControllerBase
    {
        private readonly IPhotographerService _photographerService;

        public PhotographersController(
            IPhotographerService photographerService)
        {
            _photographerService = photographerService;
        }


        // create a new photographer by studio owner
        [HttpPost]
        [Authorize(Roles = "StudioOwner")]
        public async Task<IActionResult> Create(
            [FromBody] CreatePhotographerDto dto)
        {
            var photographer =
                await _photographerService.CreateAsync(dto);

            return Ok(photographer);
        }

        
        // GET MY BOOKINGS
        // Photographer only
       
        [HttpGet("my-bookings")]
        [Authorize(Roles = "Photographer")]
        public async Task<IActionResult> GetMyBookings()
        {
            // Get UserId from JWT
            var userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new
                {
                    message = "User ID not found."
                });
            }

            var userId =
                int.Parse(userIdClaim);


            // Get bookings for this photographer
            var bookings =
                await _photographerService
                    .GetMyBookingsAsync(userId);

            return Ok(bookings);
        }


    }
}