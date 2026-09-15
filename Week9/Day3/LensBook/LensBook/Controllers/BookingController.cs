
using LensBook.Dto_s.BookingDto_s;
using LensBook.Examples;
using LensBook.Services.Interfaces;
using LensBook.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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
        /// <summary>
        /// Creates a new booking for the authenticated customer.
        /// </summary>
        /// <param name="dto">
        /// The booking information including the selected photographer, session type, and booking details.
        /// </param>
        /// <response code="201">
        /// The booking was created successfully.
        /// </response>
        /// <response code="400">
        /// The booking data is invalid or the requested booking cannot be created.
        /// </response>
        /// <response code="401">
        /// The user is not authenticated.
        /// </response>
        /// <response code="403">
        /// The authenticated user is not a customer.
        /// </response>

        [Authorize(Roles = "Customer")]
        [HttpPost]

        [SwaggerRequestExample(
    typeof(CreateBookingDto),
    typeof(CreateBookingExample))]
        public async Task<ActionResult<BookingResponseDto>> Create(
       CreateBookingDto dto)
        {
            var result =
                await _bookingService
                    .CreateAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                result);
        }

        /// <summary>
        /// Updates the status of an existing booking.
        /// </summary>
        /// <param name="bookingId">
        /// The unique identifier of the booking.
        /// </param>
        /// <param name="dto">
        /// The new status for the booking.
        /// </param>
        /// <response code="200">
        /// The booking status was updated successfully.
        /// </response>
        /// <response code="400">
        /// The booking status or request data is invalid.
        /// </response>
        /// <response code="401">
        /// The user is not authenticated.
        /// </response>
        /// <response code="404">
        /// The specified booking was not found.
        /// </response>
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

