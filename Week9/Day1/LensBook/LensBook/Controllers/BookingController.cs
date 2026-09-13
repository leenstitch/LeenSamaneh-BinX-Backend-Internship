
using LensBook.Dto_s.BookingDto_s;
using LensBook.Services.Interfaces;
using LensBook.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LensBook.Controllers
{
    [ApiController]
    [Route("api/v1/Booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(
            IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        
        // CREATE BOOKING
        // Customer only
      

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateBookingDto dto)
        {
            var result =
                await _bookingService
                    .CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                result);
        }


        [HttpPatch("{bookingId}/status")]
        [Authorize]
        public async Task<ActionResult<BookingResponseDto>> UpdateStatus(
             int bookingId,
             UpdateBookingStatusDto dto)
        {
            var result =
                await _bookingService
                    .UpdateStatusAsync(
                        bookingId,
                        dto);

            return Ok(result);
        }

    }
}

