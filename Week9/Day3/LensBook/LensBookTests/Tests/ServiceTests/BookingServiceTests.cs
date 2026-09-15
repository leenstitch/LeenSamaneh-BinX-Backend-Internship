using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LensBook.DATA;
using LensBook.Dto_s.BookingDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services;
using LensBook.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LensBookTests.Tests.ServiceTests
{
    public class BookingServiceTests : IDisposable
    {
        private readonly Mock<IBookingRepository>
            _bookingRepositoryMock;

        private readonly Mock<ISessionTypeRepository>
            _sessionTypeRepositoryMock;

        private readonly Mock<IPhotographerRepository>
            _photographerRepositoryMock;

        private readonly Mock<IExternalScheduleRepository>
            _externalScheduleRepositoryMock;

        private readonly Mock<INotificationService>
            _notificationServiceMock;

        private readonly Mock<IHttpContextAccessor>
            _httpContextAccessorMock;

        private readonly ApplicationDbContext _context;

        private readonly SqliteConnection _connection;

        private readonly BookingService _bookingService;


        public BookingServiceTests()
        {
            // ==================================================
            // SQLite In-Memory Database
            // ==================================================

            _connection =
                new SqliteConnection(
                    "DataSource=:memory:");

            _connection.Open();


            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(_connection)
                    .Options;


            _context =
                new ApplicationDbContext(options);


            _context.Database.EnsureCreated();


            // ==================================================
            // Mocks
            // ==================================================

            _bookingRepositoryMock =
                new Mock<IBookingRepository>();


            _sessionTypeRepositoryMock =
                new Mock<ISessionTypeRepository>();


            _photographerRepositoryMock =
                new Mock<IPhotographerRepository>();


            _externalScheduleRepositoryMock =
                new Mock<IExternalScheduleRepository>();


            _notificationServiceMock =
                new Mock<INotificationService>();


            _httpContextAccessorMock =
                new Mock<IHttpContextAccessor>();


            // ==================================================
            // Authenticated Customer
            // ==================================================

            var claims =
                new List<Claim>
                {
                    new Claim(
                        "CustomerId",
                        "10")
                };


            var identity =
                new ClaimsIdentity(
                    claims,
                    "TestAuthentication");


            var principal =
                new ClaimsPrincipal(identity);


            var httpContext =
                new DefaultHttpContext
                {
                    User = principal
                };


            _httpContextAccessorMock
                .Setup(x =>
                    x.HttpContext)
                .Returns(httpContext);


            // ==================================================
            // Create Service
            // ==================================================

            _bookingService =
                new BookingService(
                    _bookingRepositoryMock.Object,
                    _sessionTypeRepositoryMock.Object,
                    _photographerRepositoryMock.Object,
                    _httpContextAccessorMock.Object,
                    _externalScheduleRepositoryMock.Object,
                    _notificationServiceMock.Object,
                    _context);
        }


        // ======================================================
        // CREATE BOOKING
        // HAPPY PATH
        // ======================================================

        [Fact]
        public async Task CreateAsync_ValidBooking_ReturnsBookingResponse()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var startTime =
                DateTime.UtcNow.AddHours(2);


            var dto =
                new CreateBookingDto
                {
                    PhotographerId = 5,

                    SessionTypeId = 3,

                    StartTime = startTime,

                    Notes = "Outdoor photoshoot"
                };


            var photographer =
                new Photographer
                {
                    PhotographerId = 5,

                    UserId = 20
                };


            var sessionType =
                new SessionType
                {
                    SessionTypeId = 3,

                    DurationInMinutes = 60
                };


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.PhotographerId))
                .ReturnsAsync(photographer);


            _sessionTypeRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.SessionTypeId))
                .ReturnsAsync(sessionType);


            _bookingRepositoryMock
                .Setup(x =>
                    x.HasOverlappingBookingAsync(
                        dto.PhotographerId,
                        dto.StartTime,
                        dto.StartTime.AddMinutes(
                            sessionType.DurationInMinutes)))
                .ReturnsAsync(false);


            _externalScheduleRepositoryMock
                .Setup(x =>
                    x.HasOverlappingScheduleAsync(
                        dto.PhotographerId,
                        dto.StartTime,
                        dto.StartTime.AddMinutes(
                            sessionType.DurationInMinutes)))
                .ReturnsAsync(false);


            _bookingRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<Booking>()))
                .Returns(Task.CompletedTask);


            _notificationServiceMock
                .Setup(x =>
                    x.CreateAsync(
                        photographer.UserId,
                        "New Booking Request",
                        "You have received a new booking request."))
                .Returns(Task.CompletedTask);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var result =
                await _bookingService
                    .CreateAsync(dto);


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.NotNull(result);


            Assert.Equal(
                10,
                result.CustomerId);


            Assert.Equal(
                dto.PhotographerId,
                result.PhotographerId);


            Assert.Equal(
                dto.SessionTypeId,
                result.SessionTypeId);


            Assert.Equal(
                dto.StartTime,
                result.StartTime);


            Assert.Equal(
                dto.StartTime.AddMinutes(60),
                result.EndTime);


            Assert.Equal(
                "Pending",
                result.Status);


            Assert.Equal(
                dto.Notes,
                result.Notes);


            // --------------------------------------------------
            // Verify booking was added
            // --------------------------------------------------

            _bookingRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.Is<Booking>(
                            b =>
                                b.CustomerId == 10 &&
                                b.PhotographerId == 5 &&
                                b.SessionTypeId == 3 &&
                                b.StartTime == dto.StartTime &&
                                b.EndTime ==
                                    dto.StartTime.AddMinutes(60) &&
                                b.Status ==
                                    Booking.BookingStatus.Pending &&
                                b.Notes == dto.Notes)),
                Times.Once);


            // --------------------------------------------------
            // Verify notification
            // --------------------------------------------------

            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        photographer.UserId,
                        "New Booking Request",
                        "You have received a new booking request."),
                Times.Once);


            // --------------------------------------------------
            // Verify overlap checks
            // --------------------------------------------------

            _bookingRepositoryMock.Verify(
                x =>
                    x.HasOverlappingBookingAsync(
                        5,
                        dto.StartTime,
                        dto.StartTime.AddMinutes(60)),
                Times.Once);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.HasOverlappingScheduleAsync(
                        5,
                        dto.StartTime,
                        dto.StartTime.AddMinutes(60)),
                Times.Once);
        }


        // ======================================================
        // CUSTOMER CLAIM NOT FOUND
        // ======================================================

        [Fact]
        public async Task CreateAsync_CustomerClaimMissing_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var identity =
                new ClaimsIdentity();


            var principal =
                new ClaimsPrincipal(identity);


            var httpContext =
                new DefaultHttpContext
                {
                    User = principal
                };


            _httpContextAccessorMock
                .Setup(x =>
                    x.HttpContext)
                .Returns(httpContext);


            var dto =
                new CreateBookingDto
                {
                    PhotographerId = 5,

                    SessionTypeId = 3,

                    StartTime =
                        DateTime.UtcNow.AddHours(2),

                    Notes = "Test"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _bookingService
                            .CreateAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Customer information was not found.",
                exception.Message);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()),
                Times.Never);


            _bookingRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Booking>()),
                Times.Never);
        }


        // ======================================================
        // INVALID CUSTOMER CLAIM
        // ======================================================

        [Fact]
        public async Task CreateAsync_InvalidCustomerClaim_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var claims =
                new List<Claim>
                {
                    new Claim(
                        "CustomerId",
                        "invalid-id")
                };


            var identity =
                new ClaimsIdentity(
                    claims,
                    "TestAuthentication");


            var principal =
                new ClaimsPrincipal(identity);


            var httpContext =
                new DefaultHttpContext
                {
                    User = principal
                };


            _httpContextAccessorMock
                .Setup(x =>
                    x.HttpContext)
                .Returns(httpContext);


            var dto =
                new CreateBookingDto
                {
                    PhotographerId = 5,

                    SessionTypeId = 3,

                    StartTime =
                        DateTime.UtcNow.AddHours(2),

                    Notes = "Test"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _bookingService
                            .CreateAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Invalid customer information.",
                exception.Message);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()),
                Times.Never);
        }


        // ======================================================
        // PHOTOGRAPHER NOT FOUND
        // ======================================================

        [Fact]
        public async Task CreateAsync_PhotographerNotFound_ThrowsKeyNotFoundException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var dto =
                new CreateBookingDto
                {
                    PhotographerId = 999,

                    SessionTypeId = 3,

                    StartTime =
                        DateTime.UtcNow.AddHours(2),

                    Notes = "Test"
                };


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.PhotographerId))
                .ReturnsAsync(
                    (Photographer?)null);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    KeyNotFoundException>(
                    () =>
                        _bookingService
                            .CreateAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Photographer not found.",
                exception.Message);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()),
                Times.Never);


            _bookingRepositoryMock.Verify(
                x =>
                    x.HasOverlappingBookingAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()),
                Times.Never);


            _bookingRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Booking>()),
                Times.Never);
        }


        // ======================================================
        // SESSION TYPE NOT FOUND
        // ======================================================

        [Fact]
        public async Task CreateAsync_SessionTypeNotFound_ThrowsKeyNotFoundException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var dto =
                new CreateBookingDto
                {
                    PhotographerId = 5,

                    SessionTypeId = 999,

                    StartTime =
                        DateTime.UtcNow.AddHours(2),

                    Notes = "Test"
                };


            var photographer =
                new Photographer
                {
                    PhotographerId = 5,

                    UserId = 20
                };


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.PhotographerId))
                .ReturnsAsync(photographer);


            _sessionTypeRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.SessionTypeId))
                .ReturnsAsync(
                    (SessionType?)null);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    KeyNotFoundException>(
                    () =>
                        _bookingService
                            .CreateAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Session type not found.",
                exception.Message);


            _bookingRepositoryMock.Verify(
                x =>
                    x.HasOverlappingBookingAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()),
                Times.Never);


            _bookingRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Booking>()),
                Times.Never);
        }


        // ======================================================
        // START TIME IN THE PAST
        // ======================================================

        [Fact]
        public async Task CreateAsync_StartTimeInPast_ThrowsArgumentException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var dto =
                new CreateBookingDto
                {
                    PhotographerId = 5,

                    SessionTypeId = 3,

                    StartTime =
                        DateTime.UtcNow.AddHours(-1),

                    Notes = "Test"
                };


            var photographer =
                new Photographer
                {
                    PhotographerId = 5,

                    UserId = 20
                };


            var sessionType =
                new SessionType
                {
                    SessionTypeId = 3,

                    DurationInMinutes = 60
                };


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.PhotographerId))
                .ReturnsAsync(photographer);


            _sessionTypeRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.SessionTypeId))
                .ReturnsAsync(sessionType);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    ArgumentException>(
                    () =>
                        _bookingService
                            .CreateAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Booking start time must be in the future.",
                exception.Message);


            _bookingRepositoryMock.Verify(
                x =>
                    x.HasOverlappingBookingAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()),
                Times.Never);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.HasOverlappingScheduleAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()),
                Times.Never);


            _bookingRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Booking>()),
                Times.Never);
        }


        // ======================================================
        // OVERLAPPING BOOKING
        // ======================================================

        [Fact]
        public async Task CreateAsync_OverlappingBooking_ThrowsInvalidOperationException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var startTime =
                DateTime.UtcNow.AddHours(2);


            var dto =
                new CreateBookingDto
                {
                    PhotographerId = 5,

                    SessionTypeId = 3,

                    StartTime = startTime,

                    Notes = "Test"
                };


            var photographer =
                new Photographer
                {
                    PhotographerId = 5,

                    UserId = 20
                };


            var sessionType =
                new SessionType
                {
                    SessionTypeId = 3,

                    DurationInMinutes = 60
                };


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(5))
                .ReturnsAsync(photographer);


            _sessionTypeRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(3))
                .ReturnsAsync(sessionType);


            _bookingRepositoryMock
                .Setup(x =>
                    x.HasOverlappingBookingAsync(
                        5,
                        startTime,
                        startTime.AddMinutes(60)))
                .ReturnsAsync(true);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    InvalidOperationException>(
                    () =>
                        _bookingService
                            .CreateAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Photographer is already booked for this time.",
                exception.Message);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.HasOverlappingScheduleAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()),
                Times.Never);


            _bookingRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Booking>()),
                Times.Never);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                Times.Never);
        }


        // ======================================================
        // EXTERNAL SCHEDULE OVERLAP
        // ======================================================

        [Fact]
        public async Task CreateAsync_ExternalScheduleOverlap_ThrowsInvalidOperationException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var startTime =
                DateTime.UtcNow.AddHours(2);


            var dto =
                new CreateBookingDto
                {
                    PhotographerId = 5,

                    SessionTypeId = 3,

                    StartTime = startTime,

                    Notes = "Test"
                };


            var photographer =
                new Photographer
                {
                    PhotographerId = 5,

                    UserId = 20
                };


            var sessionType =
                new SessionType
                {
                    SessionTypeId = 3,

                    DurationInMinutes = 60
                };


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(5))
                .ReturnsAsync(photographer);


            _sessionTypeRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(3))
                .ReturnsAsync(sessionType);


            _bookingRepositoryMock
                .Setup(x =>
                    x.HasOverlappingBookingAsync(
                        5,
                        startTime,
                        startTime.AddMinutes(60)))
                .ReturnsAsync(false);


            _externalScheduleRepositoryMock
                .Setup(x =>
                    x.HasOverlappingScheduleAsync(
                        5,
                        startTime,
                        startTime.AddMinutes(60)))
                .ReturnsAsync(true);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    InvalidOperationException>(
                    () =>
                        _bookingService
                            .CreateAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Photographer is unavailable during this time.",
                exception.Message);


            _bookingRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Booking>()),
                Times.Never);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                Times.Never);
        }


        // ======================================================
        // UPDATE BOOKING STATUS
        // ======================================================


        // ======================================================
        // Helper: Set Authenticated User
        // ======================================================

        private void SetAuthenticatedUser(
            int? customerId = null,
            int? photographerId = null)
        {
            var claims =
                new List<Claim>();


            if (customerId.HasValue)
            {
                claims.Add(
                    new Claim(
                        "CustomerId",
                        customerId.Value.ToString()));
            }


            if (photographerId.HasValue)
            {
                claims.Add(
                    new Claim(
                        "PhotographerId",
                        photographerId.Value.ToString()));
            }


            var identity =
                new ClaimsIdentity(
                    claims,
                    "TestAuthentication");


            var principal =
                new ClaimsPrincipal(identity);


            var httpContext =
                new DefaultHttpContext
                {
                    User = principal
                };


            _httpContextAccessorMock
                .Setup(x =>
                    x.HttpContext)
                .Returns(httpContext);
        }


        // ======================================================
        // Helper: Set Unauthenticated User
        // ======================================================

        private void SetUnauthenticatedUser()
        {
            var identity =
                new ClaimsIdentity();


            var principal =
                new ClaimsPrincipal(identity);


            var httpContext =
                new DefaultHttpContext
                {
                    User = principal
                };


            _httpContextAccessorMock
                .Setup(x =>
                    x.HttpContext)
                .Returns(httpContext);
        }


        // ======================================================
        // Helper: Create Test Booking
        // ======================================================

        private Booking CreateTestBooking(
            Booking.BookingStatus status =
                Booking.BookingStatus.Pending)
        {
            return new Booking
            {
                BookingId = 1,

                CustomerId = 10,

                PhotographerId = 20,

                SessionTypeId = 30,

                StartTime =
                    DateTime.UtcNow.AddHours(2),

                EndTime =
                    DateTime.UtcNow.AddHours(3),

                Status = status,

                Notes = "Test booking",

                Customer = new Customer
                {
                    CustomerId = 10,

                    UserId = 100
                },

                Photographer = new Photographer
                {
                    PhotographerId = 20,

                    UserId = 200
                }
            };
        }


        // ======================================================
        // PHOTOGRAPHER CONFIRMS BOOKING
        // ======================================================

        [Fact]
        public async Task UpdateStatusAsync_PhotographerConfirmsPendingBooking_ReturnsConfirmed()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var booking =
                CreateTestBooking(
                    Booking.BookingStatus.Pending);


            _bookingRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(booking);


            SetAuthenticatedUser(
                photographerId: 20);


            var dto =
                new UpdateBookingStatusDto
                {
                    Status = "Confirmed"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var result =
                await _bookingService
                    .UpdateStatusAsync(
                        1,
                        dto);


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.NotNull(result);


            Assert.Equal(
                "Confirmed",
                result.Status);


            Assert.Equal(
                Booking.BookingStatus.Confirmed,
                booking.Status);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        100,
                        "Booking Confirmed",
                        "Your booking has been confirmed by the photographer."),
                Times.Once);
        }


        // ======================================================
        // PHOTOGRAPHER REJECTS BOOKING
        // ======================================================

        [Fact]
        public async Task UpdateStatusAsync_PhotographerRejectsPendingBooking_ReturnsRejected()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var booking =
                CreateTestBooking(
                    Booking.BookingStatus.Pending);


            _bookingRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(booking);


            SetAuthenticatedUser(
                photographerId: 20);


            var dto =
                new UpdateBookingStatusDto
                {
                    Status = "Rejected"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var result =
                await _bookingService
                    .UpdateStatusAsync(
                        1,
                        dto);


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.NotNull(result);


            Assert.Equal(
                "Rejected",
                result.Status);


            Assert.Equal(
                Booking.BookingStatus.Rejected,
                booking.Status);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        100,
                        "Booking Rejected",
                        "Your booking has been rejected by the photographer."),
                Times.Once);
        }


        // ======================================================
        // CUSTOMER CANCELS BOOKING
        // ======================================================

        [Fact]
        public async Task UpdateStatusAsync_CustomerCancelsPendingBooking_ReturnsCancelled()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var booking =
                CreateTestBooking(
                    Booking.BookingStatus.Pending);


            _bookingRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(booking);


            SetAuthenticatedUser(
                customerId: 10);


            var dto =
                new UpdateBookingStatusDto
                {
                    Status = "Cancelled"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var result =
                await _bookingService
                    .UpdateStatusAsync(
                        1,
                        dto);


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.NotNull(result);


            Assert.Equal(
                "Cancelled",
                result.Status);


            Assert.Equal(
                Booking.BookingStatus.Cancelled,
                booking.Status);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        200,
                        "Booking Cancelled",
                        "A customer has cancelled the booking."),
                Times.Once);
        }


        // ======================================================
        // PHOTOGRAPHER CANCELS BOOKING
        // ======================================================

        [Fact]
        public async Task UpdateStatusAsync_PhotographerCancelsConfirmedBooking_ReturnsCancelled()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var booking =
                CreateTestBooking(
                    Booking.BookingStatus.Confirmed);


            _bookingRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(booking);


            SetAuthenticatedUser(
                photographerId: 20);


            var dto =
                new UpdateBookingStatusDto
                {
                    Status = "Cancelled"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var result =
                await _bookingService
                    .UpdateStatusAsync(
                        1,
                        dto);


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.NotNull(result);


            Assert.Equal(
                "Cancelled",
                result.Status);


            Assert.Equal(
                Booking.BookingStatus.Cancelled,
                booking.Status);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        100,
                        "Booking Cancelled",
                        "The photographer has cancelled your booking."),
                Times.Once);
        }


        // ======================================================
        // UNRELATED USER CANNOT CANCEL
        // ======================================================

        [Fact]
        public async Task UpdateStatusAsync_UnrelatedUserCancelsBooking_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var booking =
                CreateTestBooking(
                    Booking.BookingStatus.Pending);


            _bookingRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(booking);


            // Booking belongs to Customer 10
            // Logged-in customer is Customer 999

            SetAuthenticatedUser(
                customerId: 999);


            var dto =
                new UpdateBookingStatusDto
                {
                    Status = "Cancelled"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _bookingService
                            .UpdateStatusAsync(
                                1,
                                dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "You can only cancel your own bookings.",
                exception.Message);


            Assert.Equal(
                Booking.BookingStatus.Pending,
                booking.Status);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                Times.Never);
        }


        // ======================================================
        // DIFFERENT PHOTOGRAPHER CANNOT CONFIRM
        // ======================================================

        [Fact]
        public async Task UpdateStatusAsync_DifferentPhotographerConfirmsBooking_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var booking =
                CreateTestBooking(
                    Booking.BookingStatus.Pending);


            _bookingRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(booking);


            // Booking belongs to Photographer 20
            // Logged-in photographer is Photographer 99

            SetAuthenticatedUser(
                photographerId: 99);


            var dto =
                new UpdateBookingStatusDto
                {
                    Status = "Confirmed"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _bookingService
                            .UpdateStatusAsync(
                                1,
                                dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "You can only modify your own bookings.",
                exception.Message);


            Assert.Equal(
                Booking.BookingStatus.Pending,
                booking.Status);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                Times.Never);
        }


        // ======================================================
        // COMPLETED BOOKING CANNOT BE MODIFIED
        // ======================================================

        [Fact]
        public async Task UpdateStatusAsync_CompletedBooking_ThrowsInvalidOperationException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var booking =
                CreateTestBooking(
                    Booking.BookingStatus.Completed);


            _bookingRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(booking);


            SetAuthenticatedUser(
                photographerId: 20);


            var dto =
                new UpdateBookingStatusDto
                {
                    Status = "Cancelled"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    InvalidOperationException>(
                    () =>
                        _bookingService
                            .UpdateStatusAsync(
                                1,
                                dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "This booking can no longer be modified.",
                exception.Message);


            Assert.Equal(
                Booking.BookingStatus.Completed,
                booking.Status);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                Times.Never);
        }


        // ======================================================
        // UNAUTHENTICATED USER
        // ======================================================

        [Fact]
        public async Task UpdateStatusAsync_UnauthenticatedUser_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var booking =
                CreateTestBooking(
                    Booking.BookingStatus.Pending);


            _bookingRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(booking);


            SetUnauthenticatedUser();


            var dto =
                new UpdateBookingStatusDto
                {
                    Status = "Cancelled"
                };


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _bookingService
                            .UpdateStatusAsync(
                                1,
                                dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "User is not authenticated.",
                exception.Message);


            Assert.Equal(
                Booking.BookingStatus.Pending,
                booking.Status);


            _notificationServiceMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                Times.Never);
        }


        // ======================================================
        // DISPOSE
        // ======================================================

        public void Dispose()
        {
            _context.Dispose();

            _connection.Dispose();
        }
    }
}