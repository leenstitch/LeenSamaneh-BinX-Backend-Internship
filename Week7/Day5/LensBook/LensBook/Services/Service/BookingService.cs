
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

       
        // CREATE BOOKING
     
        public async Task<BookingResponseDto> CreateAsync(
            CreateBookingDto dto)
        {
            

            var customerIdClaim =
                _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst("CustomerId");

            if (customerIdClaim == null)
            {
                throw new UnauthorizedAccessException(
                    "Customer information was not found.");
            }

            if (!int.TryParse(
         customerIdClaim.Value,
         out var customerId))
            {
                throw new UnauthorizedAccessException(
                    "Invalid customer information.");
            }




            var photographer =
                await _photographerRepository
                    .GetByIdAsync(
                        dto.PhotographerId);

            if (photographer == null)
            {
                throw new KeyNotFoundException(
                    "Photographer not found.");
            }


      
            var sessionType =
                await _sessionTypeRepository
                    .GetByIdAsync(
                        dto.SessionTypeId);

            if (sessionType == null)
            {
                throw new KeyNotFoundException(
                    "Session type not found.");
            }

            if (dto.StartTime <= DateTime.UtcNow)
            {
                throw new ArgumentException(
                    "Booking start time must be in the future.");
            }
            var endTime = dto.StartTime.AddMinutes(
                  sessionType.DurationInMinutes);

            var hasOverlap =
                await _bookingRepository.HasOverlappingBookingAsync(
                    dto.PhotographerId,
                    dto.StartTime,
                    endTime);

            if (hasOverlap)
            {
                throw new InvalidOperationException(
                    "Photographer is already booked for this time.");
            }


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



            await _bookingRepository
                .AddAsync(booking);

            await _bookingRepository
                .SaveChangesAsync();



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