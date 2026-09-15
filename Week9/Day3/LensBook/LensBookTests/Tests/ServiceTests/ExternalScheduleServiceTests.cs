using System.Security.Claims;
using LensBook.DATA;
using LensBook.Dto_s.ExternalScheduleDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LensBookTests.Tests.ServiceTests
{
    public class ExternalScheduleServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly SqliteConnection _connection;

        private readonly Mock<IExternalScheduleRepository>
            _externalScheduleRepositoryMock;

        private readonly Mock<IHttpContextAccessor>
            _httpContextAccessorMock;

        private readonly ExternalScheduleService _service;


        public ExternalScheduleServiceTests()
        {
            // =========================================
            // Create SQLite In-Memory Database
            // =========================================

            _connection =
                new SqliteConnection("DataSource=:memory:");

            _connection.Open();

            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(_connection)
                    .Options;

            _context =
                new ApplicationDbContext(options);

            _context.Database.EnsureCreated();


            // =========================================
            // Create Mocks
            // =========================================

            _externalScheduleRepositoryMock =
                new Mock<IExternalScheduleRepository>();

            _httpContextAccessorMock =
                new Mock<IHttpContextAccessor>();


            // =========================================
            // Create Service
            // =========================================

            _service =
                new ExternalScheduleService(
                    _externalScheduleRepositoryMock.Object,
                    _context,
                    _httpContextAccessorMock.Object);
        }


        // =====================================================
        // Helper: Set Authenticated User
        // =====================================================

        private void SetAuthenticatedUser(string userId)
        {
            var claims =
                new List<Claim>
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        userId)
                };

            var identity =
                new ClaimsIdentity(
                    claims,
                    "TestAuthentication");

            var principal =
                new ClaimsPrincipal(identity);

            var httpContext =
                new DefaultHttpContext();

            httpContext.User = principal;

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(httpContext);
        }


        // =====================================================
        // Helper: Set Empty User
        // =====================================================

        private void SetEmptyUser()
        {
            var httpContext =
                new DefaultHttpContext();

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(httpContext);
        }


        // =====================================================
        // Helper: Add Test Photographer
        // =====================================================

        private async Task AddTestPhotographerAsync()
        {
            var user =
                new ApplicationUser
                {
                    Id = 10,
                    UserName = "photographer@test.com",
                    Email = "photographer@test.com"
                };

            _context.Users.Add(user);


            var photographer =
                new Photographer
                {
                    PhotographerId = 5,
                    UserId = 10,
                    FirstName = "Leen",
                    LastName = "Samaneh",
                    PhoneNumber = "0599000000",
                    Bio = "Test photographer"
                };

            _context.Photographers.Add(photographer);

            await _context.SaveChangesAsync();
        }


        // =====================================================
        // 1. HAPPY PATH
        // =====================================================

        [Fact]
        public async Task AddExternalScheduleAsync_ValidData_ReturnsResponse()
        {
            // Arrange

            SetAuthenticatedUser("10");

            await AddTestPhotographerAsync();


            var dto =
                new CreateExternalScheduleDto
                {
                    StartTime =
                        DateTime.UtcNow.AddDays(1),

                    EndTime =
                        DateTime.UtcNow.AddDays(1).AddHours(2),

                    Location = "Nablus",

                    Notes = "External wedding session"
                };


            _externalScheduleRepositoryMock
                .Setup(x =>
                    x.HasOverlappingScheduleAsync(
                        5,
                        dto.StartTime,
                        dto.EndTime))
                .ReturnsAsync(false);


            _externalScheduleRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<ExternalSchedule>()))
                .ReturnsAsync(
                    (ExternalSchedule schedule) =>
                    {
                        schedule.ExternalScheduleId = 100;

                        return schedule;
                    });


            // Act

            var result =
                await _service
                    .AddExternalScheduleAsync(dto);


            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                100,
                result.ExternalScheduleId);

            Assert.Equal(
                5,
                result.PhotographerId);

            Assert.Equal(
                dto.StartTime,
                result.StartTime);

            Assert.Equal(
                dto.EndTime,
                result.EndTime);

            Assert.Equal(
                dto.Location,
                result.Location);

            Assert.Equal(
                dto.Notes,
                result.Notes);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.HasOverlappingScheduleAsync(
                        5,
                        dto.StartTime,
                        dto.EndTime),
                Times.Once);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.Is<ExternalSchedule>(
                            schedule =>
                                schedule.PhotographerId == 5 &&
                                schedule.StartTime == dto.StartTime &&
                                schedule.EndTime == dto.EndTime &&
                                schedule.Location == dto.Location &&
                                schedule.Notes == dto.Notes)),
                Times.Once);
        }


        // =====================================================
        // 2. USER ID CLAIM MISSING
        // =====================================================

        [Fact]
        public async Task AddExternalScheduleAsync_UserIdClaimMissing_ThrowsUnauthorizedAccessException()
        {
            // Arrange

            SetEmptyUser();

            var dto =
                new CreateExternalScheduleDto
                {
                    StartTime =
                        DateTime.UtcNow.AddDays(1),

                    EndTime =
                        DateTime.UtcNow.AddDays(1).AddHours(2),

                    Location = "Nablus",

                    Notes = "Test"
                };


            // Act

            var exception =
                await Assert.ThrowsAsync<UnauthorizedAccessException>(
                    () =>
                        _service
                            .AddExternalScheduleAsync(dto));


            // Assert

            Assert.Equal(
                "User ID was not found in the token.",
                exception.Message);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.HasOverlappingScheduleAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()),
                Times.Never);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<ExternalSchedule>()),
                Times.Never);
        }


        // =====================================================
        // 3. INVALID USER ID CLAIM
        // =====================================================

        [Fact]
        public async Task AddExternalScheduleAsync_InvalidUserIdClaim_ThrowsUnauthorizedAccessException()
        {
            // Arrange

            SetAuthenticatedUser("abc");

            var dto =
                new CreateExternalScheduleDto
                {
                    StartTime =
                        DateTime.UtcNow.AddDays(1),

                    EndTime =
                        DateTime.UtcNow.AddDays(1).AddHours(2),

                    Location = "Nablus",

                    Notes = "Test"
                };


            // Act

            var exception =
                await Assert.ThrowsAsync<UnauthorizedAccessException>(
                    () =>
                        _service
                            .AddExternalScheduleAsync(dto));


            // Assert

            Assert.Equal(
                "Invalid User ID in token.",
                exception.Message);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<ExternalSchedule>()),
                Times.Never);
        }


        // =====================================================
        // 4. USER IS NOT A PHOTOGRAPHER
        // =====================================================

        [Fact]
        public async Task AddExternalScheduleAsync_UserIsNotPhotographer_ThrowsUnauthorizedAccessException()
        {
            // Arrange

            SetAuthenticatedUser("999");

            var dto =
                new CreateExternalScheduleDto
                {
                    StartTime =
                        DateTime.UtcNow.AddDays(1),

                    EndTime =
                        DateTime.UtcNow.AddDays(1).AddHours(2),

                    Location = "Nablus",

                    Notes = "Test"
                };


            // Act

            var exception =
                await Assert.ThrowsAsync<UnauthorizedAccessException>(
                    () =>
                        _service
                            .AddExternalScheduleAsync(dto));


            // Assert

            Assert.Equal(
                "The current user is not a photographer.",
                exception.Message);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<ExternalSchedule>()),
                Times.Never);
        }


        // =====================================================
        // 5. INVALID TIME
        // =====================================================

        [Fact]
        public async Task AddExternalScheduleAsync_StartTimeAfterEndTime_ThrowsArgumentException()
        {
            // Arrange

            SetAuthenticatedUser("10");

            await AddTestPhotographerAsync();


            var dto =
                new CreateExternalScheduleDto
                {
                    StartTime =
                        DateTime.UtcNow.AddDays(1).AddHours(3),

                    EndTime =
                        DateTime.UtcNow.AddDays(1).AddHours(1),

                    Location = "Nablus",

                    Notes = "Invalid time"
                };


            // Act

            var exception =
                await Assert.ThrowsAsync<ArgumentException>(
                    () =>
                        _service
                            .AddExternalScheduleAsync(dto));


            // Assert

            Assert.Equal(
                "Start time must be before end time.",
                exception.Message);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.HasOverlappingScheduleAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()),
                Times.Never);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<ExternalSchedule>()),
                Times.Never);
        }


        // =====================================================
        // 6. OVERLAPPING EXTERNAL SCHEDULE
        // =====================================================

        [Fact]
        public async Task AddExternalScheduleAsync_OverlappingSchedule_ThrowsInvalidOperationException()
        {
            // Arrange

            SetAuthenticatedUser("10");

            await AddTestPhotographerAsync();


            var dto =
                new CreateExternalScheduleDto
                {
                    StartTime =
                        DateTime.UtcNow.AddDays(1),

                    EndTime =
                        DateTime.UtcNow.AddDays(1).AddHours(2),

                    Location = "Nablus",

                    Notes = "Overlapping schedule"
                };


            _externalScheduleRepositoryMock
                .Setup(x =>
                    x.HasOverlappingScheduleAsync(
                        5,
                        dto.StartTime,
                        dto.EndTime))
                .ReturnsAsync(true);


            // Act

            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () =>
                        _service
                            .AddExternalScheduleAsync(dto));


            // Assert

            Assert.Equal(
                "The photographer already has an external booking during this time.",
                exception.Message);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.HasOverlappingScheduleAsync(
                        5,
                        dto.StartTime,
                        dto.EndTime),
                Times.Once);


            _externalScheduleRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<ExternalSchedule>()),
                Times.Never);
        }


        // =====================================================
        // Dispose
        // =====================================================

        public void Dispose()
        {
            _context.Dispose();

            _connection.Dispose();
        }
    }
}