
using LensBook.Dto_s.BookingDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services.Interfaces;
using LensBook.Services.IServices;

namespace LensBook.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISessionTypeRepository _sessionTypeRepository;
        private readonly IPhotographerRepository _photographerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(
            IBookingRepository bookingRepository,
            ISessionTypeRepository sessionTypeRepository,
            IPhotographerRepository photographerRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookingRepository = bookingRepository;
            _sessionTypeRepository = sessionTypeRepository;
            _photographerRepository = photographerRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // =====================================================
        // CREATE BOOKING
        // =====================================================

        public async Task<BookingResponseDto> CreateAsync(
            CreateBookingDto dto)
        {
            // -------------------------------------------------
            // 1. Get CustomerId from JWT
            // -------------------------------------------------

            var customerIdClaim =
                _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst("CustomerId");

            if (customerIdClaim == null)
            {
                throw new UnauthorizedAccessException(
                    "Customer information was not found.");
            }

            var customerId =
                int.Parse(customerIdClaim.Value);


            // -------------------------------------------------
            // 2. Check Photographer
            // -------------------------------------------------

            var photographer =
                await _photographerRepository
                    .GetByIdAsync(
                        dto.PhotographerId);

            if (photographer == null)
            {
                throw new Exception(
                    "Photographer not found.");
            }


            // -------------------------------------------------
            // 3. Check Session Type
            // -------------------------------------------------

            var sessionType =
                await _sessionTypeRepository
                    .GetByIdAsync(
                        dto.SessionTypeId);

            if (sessionType == null)
            {
                throw new Exception(
                    "Session type not found.");
            }


            // -------------------------------------------------
            // 4. Calculate End Time
            // -------------------------------------------------

            var endTime =
                dto.StartTime.AddMinutes(
                    sessionType.DurationInMinutes);


            // -------------------------------------------------
            // 5. Create Booking
            // -------------------------------------------------

            var booking = new Booking
            {
                CustomerId =
                    customerId,

                PhotographerId =
                    dto.PhotographerId,

                SessionTypeId =
                    dto.SessionTypeId,

                StartTime =
                    dto.StartTime,

                EndTime =
                    endTime,

                Status =
                    Booking.BookingStatus.Pending,

                Notes =
                    dto.Notes
            };


            // -------------------------------------------------
            // 6. Save Booking
            // -------------------------------------------------

            await _bookingRepository
                .AddAsync(booking);

            await _bookingRepository
                .SaveChangesAsync();


            // -------------------------------------------------
            // 7. Return Response
            // -------------------------------------------------

            return new BookingResponseDto
            {
                BookingId =
                    booking.BookingId,

                CustomerId =
                    booking.CustomerId,

                PhotographerId =
                    booking.PhotographerId,

                SessionTypeId =
                    booking.SessionTypeId,

                StartTime =
                    booking.StartTime,

                EndTime =
                    booking.EndTime,

                Status =
                    booking.Status.ToString(),

                Notes =
                    booking.Notes
            };
        }


       
    }
}