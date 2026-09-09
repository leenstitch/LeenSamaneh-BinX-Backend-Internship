
using LensBook.DATA;
using LensBook.Dto_s.BookingDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services.Interfaces;
using LensBook.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace LensBook.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISessionTypeRepository _sessionTypeRepository;
        private readonly IPhotographerRepository _photographerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExternalScheduleRepository _externalScheduleRepository;
        private readonly INotificationService _notificationService;
        private readonly ApplicationDbContext  _context;
        public BookingService(
            IBookingRepository bookingRepository,
            ISessionTypeRepository sessionTypeRepository,
            IPhotographerRepository photographerRepository,
            IHttpContextAccessor httpContextAccessor,
            IExternalScheduleRepository externalScheduleRepository,
            INotificationService notificationService,
            ApplicationDbContext context)
        {
            _bookingRepository = bookingRepository;
            _sessionTypeRepository = sessionTypeRepository;
            _photographerRepository = photographerRepository;
            _httpContextAccessor = httpContextAccessor;
            _externalScheduleRepository = externalScheduleRepository;
            _notificationService = notificationService;
            _context = context;
        }


        // CREATE BOOKING

        public async Task<BookingResponseDto>
            CreateAsync(
                CreateBookingDto dto)
        {
            // Get CustomerId from JWT

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


            // Get photographer

            var photographer =
                await _photographerRepository
                    .GetByIdAsync(
                        dto.PhotographerId);

            if (photographer == null)
            {
                throw new KeyNotFoundException(
                    "Photographer not found.");
            }


            // Get session type

            var sessionType =
                await _sessionTypeRepository
                    .GetByIdAsync(
                        dto.SessionTypeId);

            if (sessionType == null)
            {
                throw new KeyNotFoundException(
                    "Session type not found.");
            }


            // Check start time

            if (dto.StartTime <= DateTime.UtcNow)
            {
                throw new ArgumentException(
                    "Booking start time must be in the future.");
            }


            // Calculate end time

            var endTime =
                dto.StartTime.AddMinutes(
                    sessionType.DurationInMinutes);


            // Check existing bookings

            var hasOverlap =
                await _bookingRepository
                    .HasOverlappingBookingAsync(
                        dto.PhotographerId,
                        dto.StartTime,
                        endTime);

            if (hasOverlap)
            {
                throw new InvalidOperationException(
                    "Photographer is already booked for this time.");
            }


            // Check external schedule

            var hasExternalOverlap =
                await _externalScheduleRepository
                    .HasOverlappingScheduleAsync(
                        dto.PhotographerId,
                        dto.StartTime,
                        endTime);

            if (hasExternalOverlap)
            {
                throw new InvalidOperationException(
                    "Photographer is unavailable during this time.");
            }


            // Start transaction

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                // Create booking

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


                // Add booking

                await _bookingRepository
                    .AddAsync(booking);


                // Create notification

                await _notificationService
                    .CreateAsync(
                        photographer.UserId,
                        "New Booking Request",
                        "You have received a new booking request.");


                // Save booking + notification together

                await _context
                    .SaveChangesAsync();


                // Commit transaction

                await transaction
                    .CommitAsync();


                // Return response

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
            catch
            {
                // Rollback if anything fails

                await transaction
                    .RollbackAsync();

                throw;
            }
        }


        // UPDATE BOOKING STATUS

        public async Task<BookingResponseDto> UpdateStatusAsync(
            int bookingId,
            UpdateBookingStatusDto dto)
        {
            // Get booking

            var booking =
                await _bookingRepository
                    .GetByIdAsync(bookingId);

            if (booking == null)
            {
                throw new KeyNotFoundException(
                    "Booking not found.");
            }


            // Get current user

            var user =
                _httpContextAccessor.HttpContext?
                    .User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }


            // Get CustomerId claim

            var customerIdClaim =
                user.FindFirst("CustomerId");


            // Get PhotographerId claim

            var photographerIdClaim =
                user.FindFirst("PhotographerId");


            // Parse CustomerId if exists

            int? customerId = null;

            if (customerIdClaim != null &&
                int.TryParse(
                    customerIdClaim.Value,
                    out var parsedCustomerId))
            {
                customerId = parsedCustomerId;
            }


            // Parse PhotographerId if exists

            int? photographerId = null;

            if (photographerIdClaim != null &&
                int.TryParse(
                    photographerIdClaim.Value,
                    out var parsedPhotographerId))
            {
                photographerId = parsedPhotographerId;
            }


            // Validate requested status

            if (!Enum.TryParse(
                dto.Status,
                true,
                out Booking.BookingStatus newStatus))
            {
                throw new ArgumentException(
                    "Invalid booking status.");
            }


            // Only these three statuses are allowed

            if (newStatus != Booking.BookingStatus.Confirmed &&
                newStatus != Booking.BookingStatus.Rejected &&
                newStatus != Booking.BookingStatus.Cancelled)
            {
                throw new ArgumentException(
                    "Only Confirmed, Rejected, or Cancelled statuses are allowed.");
            }


            // Check current status

            if (booking.Status ==
                    Booking.BookingStatus.Rejected ||
                booking.Status ==
                    Booking.BookingStatus.Cancelled ||
                booking.Status ==
                    Booking.BookingStatus.Completed)
            {
                throw new InvalidOperationException(
                    "This booking can no longer be modified.");
            }


            // =========================
            // CONFIRMED
            // =========================

            if (newStatus ==
                Booking.BookingStatus.Confirmed)
            {
                // Only photographer can confirm

                if (photographerId == null)
                {
                    throw new UnauthorizedAccessException(
                        "Only the photographer can confirm a booking.");
                }


                // Photographer must own the booking

                if (booking.PhotographerId !=
                    photographerId.Value)
                {
                    throw new UnauthorizedAccessException(
                        "You can only modify your own bookings.");
                }


                // Can only confirm pending booking

                if (booking.Status !=
                    Booking.BookingStatus.Pending)
                {
                    throw new InvalidOperationException(
                        "Only pending bookings can be confirmed.");
                }
            }


            // =========================
            // REJECTED
            // =========================

            if (newStatus ==
                Booking.BookingStatus.Rejected)
            {
                // Only photographer can reject

                if (photographerId == null)
                {
                    throw new UnauthorizedAccessException(
                        "Only the photographer can reject a booking.");
                }


                // Photographer must own the booking

                if (booking.PhotographerId !=
                    photographerId.Value)
                {
                    throw new UnauthorizedAccessException(
                        "You can only modify your own bookings.");
                }


                // Can only reject pending booking

                if (booking.Status !=
                    Booking.BookingStatus.Pending)
                {
                    throw new InvalidOperationException(
                        "Only pending bookings can be rejected.");
                }
            }


            // =========================
            // CANCELLED
            // =========================

            if (newStatus ==
                Booking.BookingStatus.Cancelled)
            {
                // Customer cancellation

                if (customerId != null &&
                    booking.CustomerId ==
                    customerId.Value)
                {
                    // Customer is allowed to cancel
                }


                // Photographer cancellation

                else if (photographerId != null &&
                         booking.PhotographerId ==
                         photographerId.Value)
                {
                    // Photographer is allowed to cancel
                }


                // User is not related to booking

                else
                {
                    throw new UnauthorizedAccessException(
                        "You can only cancel your own bookings.");
                }


                // Only Pending or Confirmed can be cancelled

                if (booking.Status !=
                        Booking.BookingStatus.Pending &&
                    booking.Status !=
                        Booking.BookingStatus.Confirmed)
                {
                    throw new InvalidOperationException(
                        "Only pending or confirmed bookings can be cancelled.");
                }
            }


            // =========================
            // START TRANSACTION
            // =========================

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                // Update booking status

                booking.Status = newStatus;


                // =========================
                // CREATE NOTIFICATION
                // =========================


                // Photographer confirmed booking
                if (newStatus ==
                    Booking.BookingStatus.Confirmed)
                {
                    await _notificationService
                        .CreateAsync(
                            booking.Customer.UserId,
                            "Booking Confirmed",
                            "Your booking has been confirmed by the photographer.");
                }


                // Photographer rejected booking
                else if (newStatus ==
                         Booking.BookingStatus.Rejected)
                {
                    await _notificationService
                        .CreateAsync(
                            booking.Customer.UserId,
                            "Booking Rejected",
                            "Your booking has been rejected by the photographer.");
                }


                // Booking cancelled
                else if (newStatus ==
                         Booking.BookingStatus.Cancelled)
                {
                    // Customer cancelled booking

                    if (customerId != null &&
                        booking.CustomerId ==
                        customerId.Value)
                    {
                        await _notificationService
                            .CreateAsync(
                                booking.Photographer.UserId,
                                "Booking Cancelled",
                                "A customer has cancelled the booking.");
                    }


                    // Photographer cancelled booking

                    else if (photographerId != null &&
                             booking.PhotographerId ==
                             photographerId.Value)
                    {
                        await _notificationService
                            .CreateAsync(
                                booking.Customer.UserId,
                                "Booking Cancelled",
                                "The photographer has cancelled your booking.");
                    }
                }


                // Save booking and notification

                await _context.SaveChangesAsync();


                // Commit transaction

                await transaction.CommitAsync();


                // Return response

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
            catch
            {
                // Rollback everything if something fails

                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}