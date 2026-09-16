using LensBook.DATA;
using LensBook.Dto_s.PhotographerDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using static LensBook.Models.Booking;

namespace LensBookTests.Tests.ServiceTests
{
    public class PhotographerServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly SqliteConnection _connection;

        private readonly Mock<UserManager<ApplicationUser>>
            _userManagerMock;

        private readonly Mock<IPhotographerRepository>
            _photographerRepositoryMock;

        private readonly PhotographerService _service;


        public PhotographerServiceTests()
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
            // Create UserManager Mock
            // =========================================

            var userStoreMock =
                new Mock<IUserStore<ApplicationUser>>();

            _userManagerMock =
                new Mock<UserManager<ApplicationUser>>(
                    userStoreMock.Object,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);


            // =========================================
            // Create Repository Mock
            // =========================================

            _photographerRepositoryMock =
                new Mock<IPhotographerRepository>();


            // =========================================
            // Create Service
            // =========================================

            _service =
                new PhotographerService(
                    _userManagerMock.Object,
                    _photographerRepositoryMock.Object,
                    _context);
        }


        // =====================================================
        // 1. VALID DATA
        // =====================================================

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsPhotographerResponse()
        {
            // Arrange

            var dto =
                new CreatePhotographerDto
                {
                    Email = "photographer@test.com",
                    Password = "Password123!",
                    PhoneNumber = "0599000000",
                    FirstName = "Leen",
                    LastName = "Samaneh",
                    Bio = "Professional photographer"
                };


            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(
                    (ApplicationUser?)null);


            _userManagerMock
     .Setup(x =>
         x.CreateAsync(
             It.IsAny<ApplicationUser>(),
             dto.Password))
     .Callback(
         (ApplicationUser user, string password) =>
         {
             user.Id = 10;
         })
     .ReturnsAsync(
         IdentityResult.Success);


            _userManagerMock
                .Setup(x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Photographer"))
                .ReturnsAsync(
                    IdentityResult.Success);


            _photographerRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<Photographer>()))
                .Callback(
                    (Photographer photographer) =>
                    {
                        photographer.PhotographerId = 1;
                    })
                .Returns(Task.CompletedTask);


            _photographerRepositoryMock
                .Setup(x =>
                    x.SaveChangesAsync())
                .Returns(Task.CompletedTask);


            // Act

            var result =
                await _service.CreateAsync(dto);


            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.PhotographerId);

            Assert.Equal(
                10,
                result.UserId);

            Assert.Equal(
                dto.FirstName,
                result.FirstName);

            Assert.Equal(
                dto.LastName,
                result.LastName);

            Assert.Equal(
                dto.PhoneNumber,
                result.PhoneNumber);

            Assert.Equal(
                dto.Bio,
                result.Bio);


            _userManagerMock.Verify(
                x =>
                    x.FindByEmailAsync(dto.Email),
                Times.Once);


            _userManagerMock.Verify(
                x =>
                    x.CreateAsync(
                        It.Is<ApplicationUser>(
                            user =>
                                user.Email == dto.Email &&
                                user.UserName == dto.Email &&
                                user.PhoneNumber == dto.PhoneNumber),
                        dto.Password),
                Times.Once);


            _userManagerMock.Verify(
                x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Photographer"),
                Times.Once);


            _photographerRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.Is<Photographer>(
                            photographer =>
                                photographer.UserId == 10 &&
                                photographer.FirstName == dto.FirstName &&
                                photographer.LastName == dto.LastName &&
                                photographer.PhoneNumber == dto.PhoneNumber &&
                                photographer.Bio == dto.Bio)),
                Times.Once);


            _photographerRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Once);
        }


        // =====================================================
        // 2. EMAIL ALREADY EXISTS
        // =====================================================

        [Fact]
        public async Task CreateAsync_EmailAlreadyExists_ThrowsArgumentException()
        {
            // Arrange

            var dto =
                new CreatePhotographerDto
                {
                    Email = "existing@test.com",
                    Password = "Password123!",
                    PhoneNumber = "0599000000",
                    FirstName = "Leen",
                    LastName = "Samaneh",
                    Bio = "Photographer"
                };


            var existingUser =
                new ApplicationUser
                {
                    Id = 10,
                    Email = dto.Email,
                    UserName = dto.Email
                };


            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(existingUser);


            // Act

            var exception =
                await Assert.ThrowsAsync<ArgumentException>(
                    () =>
                        _service.CreateAsync(dto));


            // Assert

            Assert.Equal(
                "A user with this email already exists.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<ApplicationUser>(),
                        It.IsAny<string>()),
                Times.Never);


            _userManagerMock.Verify(
                x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        It.IsAny<string>()),
                Times.Never);


            _photographerRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Photographer>()),
                Times.Never);


            _photographerRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // =====================================================
        // 3. USER CREATION FAILS
        // =====================================================

        [Fact]
        public async Task CreateAsync_UserCreationFails_ThrowsArgumentException()
        {
            // Arrange

            var dto =
                new CreatePhotographerDto
                {
                    Email = "photographer@test.com",
                    Password = "Password123!",
                    PhoneNumber = "0599000000",
                    FirstName = "Leen",
                    LastName = "Samaneh",
                    Bio = "Photographer"
                };


            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(
                    (ApplicationUser?)null);


            var identityErrors =
                new[]
                {
                    new IdentityError
                    {
                        Description =
                            "Password is too weak."
                    }
                };


            _userManagerMock
                .Setup(x =>
                    x.CreateAsync(
                        It.IsAny<ApplicationUser>(),
                        dto.Password))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        identityErrors));


            // Act

            var exception =
                await Assert.ThrowsAsync<ArgumentException>(
                    () =>
                        _service.CreateAsync(dto));


            // Assert

            Assert.Contains(
                "Failed to create photographer account:",
                exception.Message);

            Assert.Contains(
                "Password is too weak.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        It.IsAny<string>()),
                Times.Never);


            _photographerRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Photographer>()),
                Times.Never);


            _photographerRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // =====================================================
        // 4. ROLE ASSIGNMENT FAILS
        // =====================================================

        [Fact]
        public async Task CreateAsync_RoleAssignmentFails_ThrowsKeyNotFoundException()
        {
            // Arrange

            var dto =
                new CreatePhotographerDto
                {
                    Email = "photographer@test.com",
                    Password = "Password123!",
                    PhoneNumber = "0599000000",
                    FirstName = "Leen",
                    LastName = "Samaneh",
                    Bio = "Photographer"
                };


            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(
                    (ApplicationUser?)null);


            _userManagerMock
                .Setup(x =>
                    x.CreateAsync(
                        It.IsAny<ApplicationUser>(),
                        dto.Password))
                .ReturnsAsync(
                    IdentityResult.Success);


            var identityErrors =
                new[]
                {
                    new IdentityError
                    {
                        Description =
                            "Photographer role does not exist."
                    }
                };


            _userManagerMock
                .Setup(x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Photographer"))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        identityErrors));


            // Act

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () =>
                        _service.CreateAsync(dto));


            // Assert

            Assert.Contains(
                "Failed to assign Photographer role:",
                exception.Message);

            Assert.Contains(
                "Photographer role does not exist.",
                exception.Message);


            _photographerRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<Photographer>()),
                Times.Never);


            _photographerRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
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

        // =====================================================
        // 5. GET MY BOOKINGS - VALID DATA
        // =====================================================

        [Fact]
        public async Task GetMyBookingsAsync_ValidPhotographer_ReturnsBookings()
        {
            // Arrange

            var photographer =
                new Photographer
                {
                    PhotographerId = 5,
                    UserId = 10,
                    FirstName = "Leen",
                    LastName = "Samaneh"
                };


            var bookings =
                new List<Booking>
                {
            new Booking
            {
                BookingId = 1,
                CustomerId = 20,
                PhotographerId = 5,
                SessionTypeId = 3,
                StartTime =
                    new DateTime(2026, 10, 10, 10, 0, 0),
                EndTime =
                    new DateTime(2026, 10, 10, 12, 0, 0),
                Status = BookingStatus.Confirmed,
                Notes = "Wedding session"
            },

            new Booking
            {
                BookingId = 2,
                CustomerId = 21,
                PhotographerId = 5,
                SessionTypeId = 4,
                StartTime =
                    new DateTime(2026, 10, 11, 14, 0, 0),
                EndTime =
                    new DateTime(2026, 10, 11, 16, 0, 0),
                Status = BookingStatus.Pending,
                Notes = "Portrait session"
            }
                };


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(10))
                .ReturnsAsync(photographer);


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetMyBookingsAsync(5))
                .ReturnsAsync(bookings);


            // Act

            var result =
                await _service.GetMyBookingsAsync(10);


            // Assert

            Assert.NotNull(result);

            var resultList =
                result.ToList();


            Assert.Equal(
                2,
                resultList.Count);


            Assert.Equal(
                1,
                resultList[0].BookingId);

            Assert.Equal(
                20,
                resultList[0].CustomerId);

            Assert.Equal(
                5,
                resultList[0].PhotographerId);

            Assert.Equal(
                3,
                resultList[0].SessionTypeId);

            Assert.Equal(
                "Confirmed",
                resultList[0].Status);

            Assert.Equal(
                "Wedding session",
                resultList[0].Notes);


            Assert.Equal(
                2,
                resultList[1].BookingId);

            Assert.Equal(
                21,
                resultList[1].CustomerId);

            Assert.Equal(
                "Pending",
                resultList[1].Status);

            Assert.Equal(
                "Portrait session",
                resultList[1].Notes);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(10),
                Times.Once);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetMyBookingsAsync(5),
                Times.Once);
        }


        // =====================================================
        // 6. PHOTOGRAPHER NOT FOUND
        // =====================================================

        [Fact]
        public async Task GetMyBookingsAsync_PhotographerNotFound_ThrowsException()
        {
            // Arrange

            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(10))
                .ReturnsAsync(
                    (Photographer?)null);


            // Act

            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service.GetMyBookingsAsync(10));


            // Assert

            Assert.Equal(
                "Photographer profile not found.",
                exception.Message);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(10),
                Times.Once);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetMyBookingsAsync(
                        It.IsAny<int>()),
                Times.Never);
        }


        // =====================================================
        // 7. PHOTOGRAPHER HAS NO BOOKINGS
        // =====================================================

        [Fact]
        public async Task GetMyBookingsAsync_NoBookings_ReturnsEmptyList()
        {
            // Arrange

            var photographer =
                new Photographer
                {
                    PhotographerId = 5,
                    UserId = 10,
                    FirstName = "Leen",
                    LastName = "Samaneh"
                };


            var bookings =
                new List<Booking>();


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(10))
                .ReturnsAsync(photographer);


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetMyBookingsAsync(5))
                .ReturnsAsync(bookings);


            // Act

            var result =
                await _service.GetMyBookingsAsync(10);


            // Assert

            Assert.NotNull(result);

            var resultList =
                result.ToList();


            Assert.Empty(resultList);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(10),
                Times.Once);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetMyBookingsAsync(5),
                Times.Once);
        }
        // =====================================================
        // 8. PROJECTION - VALID DATA
        // =====================================================

        [Fact]
        public async Task GetMyBookingsAsyncByUsingProjection_ValidPhotographer_ReturnsBookings()
        {
            // Arrange


            // =====================================================
            // Create Photographer User
            // =====================================================

            var photographerUser =
                new ApplicationUser
                {
                    Id = 10,
                    UserName = "photographer@test.com",
                    Email = "photographer@test.com"
                };

            _context.Users.Add(photographerUser);


            // =====================================================
            // Create Photographer
            // =====================================================

            var photographer =
                new Photographer
                {
                    PhotographerId = 5,
                    UserId = 10,
                    FirstName = "Leen",
                    LastName = "Samaneh"
                };

            _context.Photographers.Add(photographer);


            // =====================================================
            // Create Customer User
            // =====================================================

            var customerUser =
                new ApplicationUser
                {
                    Id = 20,
                    UserName = "customer@test.com",
                    Email = "customer@test.com"
                };

            _context.Users.Add(customerUser);


            // =====================================================
            // Create Customer
            // =====================================================

            var customer =
                new Customer
                {
                    CustomerId = 20,
                    UserId = 20,
                    FirstName = "Test",
                    LastName = "Customer"
                };

            _context.Customers.Add(customer);


            // =====================================================
            // Create Session Types
            // =====================================================

            var sessionType1 =
                new SessionType
                {
                    SessionTypeId = 3,
                    Name = "Wedding Photography",
                    DurationInMinutes = 120,
                    Price = 500,
                    IsActive = true
                };

            var sessionType2 =
                new SessionType
                {
                    SessionTypeId = 4,
                    Name = "Portrait Photography",
                    DurationInMinutes = 60,
                    Price = 200,
                    IsActive = true
                };

            _context.SessionTypes.AddRange(
                sessionType1,
                sessionType2);


            // =====================================================
            // Create Bookings
            // =====================================================

            var booking1 =
                new Booking
                {
                    BookingId = 1,
                    CustomerId = 20,
                    PhotographerId = 5,
                    SessionTypeId = 3,
                    StartTime =
                        new DateTime(2026, 10, 10, 10, 0, 0),
                    EndTime =
                        new DateTime(2026, 10, 10, 12, 0, 0),
                    Status = Booking.BookingStatus.Confirmed,
                    Notes = "Wedding session"
                };


            var booking2 =
                new Booking
                {
                    BookingId = 2,
                    CustomerId = 20,
                    PhotographerId = 5,
                    SessionTypeId = 4,
                    StartTime =
                        new DateTime(2026, 10, 11, 14, 0, 0),
                    EndTime =
                        new DateTime(2026, 10, 11, 16, 0, 0),
                    Status = Booking.BookingStatus.Pending,
                    Notes = "Portrait session"
                };


            _context.Bookings.AddRange(
                booking1,
                booking2);


            // =====================================================
            // Save all required database data
            // =====================================================

            await _context.SaveChangesAsync();


            // =====================================================
            // Mock Repository
            // =====================================================

            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(10))
                .ReturnsAsync(photographer);


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetMyBookingsAsync(5))
                .ReturnsAsync(
                    new List<Booking>
                    {
                booking1,
                booking2
                    });


            // =====================================================
            // Act
            // =====================================================

            var result =
                await _service
                    .GetMyBookingsAsyncByUsingProjection(10);


            // =====================================================
            // Assert
            // =====================================================

            Assert.NotNull(result);

            var resultList =
                result.ToList();


            Assert.Equal(
                2,
                resultList.Count);


            // =====================================================
            // Assert First Booking
            // =====================================================

            Assert.Equal(
                1,
                resultList[0].BookingId);

            Assert.Equal(
                20,
                resultList[0].CustomerId);

            Assert.Equal(
                5,
                resultList[0].PhotographerId);

            Assert.Equal(
                3,
                resultList[0].SessionTypeId);

            Assert.Equal(
                "Confirmed",
                resultList[0].Status);

            Assert.Equal(
                "Wedding session",
                resultList[0].Notes);


            // =====================================================
            // Assert Second Booking
            // =====================================================

            Assert.Equal(
                2,
                resultList[1].BookingId);

            Assert.Equal(
                20,
                resultList[1].CustomerId);

            Assert.Equal(
                5,
                resultList[1].PhotographerId);

            Assert.Equal(
                4,
                resultList[1].SessionTypeId);

            Assert.Equal(
                "Pending",
                resultList[1].Status);

            Assert.Equal(
                "Portrait session",
                resultList[1].Notes);


            // =====================================================
            // Verify All Bookings Belong To Photographer
            // =====================================================

            Assert.All(
                resultList,
                booking =>
                    Assert.Equal(
                        5,
                        booking.PhotographerId));


            // =====================================================
            // Verify Repository Calls
            // =====================================================

            _photographerRepositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(10),
                Times.Once);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetMyBookingsAsync(5),
                Times.Once);
        }

        // =====================================================
        // 9. PROJECTION - PHOTOGRAPHER NOT FOUND
        // =====================================================

        [Fact]
        public async Task GetMyBookingsAsyncByUsingProjection_PhotographerNotFound_ThrowsException()
        {
            // Arrange

            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(10))
                .ReturnsAsync(
                    (Photographer?)null);


            // Act

            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .GetMyBookingsAsyncByUsingProjection(10));


            // Assert

            Assert.Equal(
                "Photographer profile not found.",
                exception.Message);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(10),
                Times.Once);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetMyBookingsAsync(
                        It.IsAny<int>()),
                Times.Never);
        }


        // =====================================================
        // 10. PROJECTION - NO BOOKINGS
        // =====================================================

        [Fact]
        public async Task GetMyBookingsAsyncByUsingProjection_NoBookings_ReturnsEmptyList()
        {
            // Arrange

            // Create required ApplicationUser
            var user =
                new ApplicationUser
                {
                    Id = 10,
                    UserName = "photographer@test.com",
                    Email = "photographer@test.com"
                };

            _context.Users.Add(user);


            // Create required Photographer
            var photographer =
                new Photographer
                {
                    PhotographerId = 5,
                    UserId = 10,
                    FirstName = "Leen",
                    LastName = "Samaneh"
                };

            _context.Photographers.Add(photographer);


            // Save required data to the database
            await _context.SaveChangesAsync();


            // Mock repository
            _photographerRepositoryMock
                .Setup(x =>
                    x.GetByUserIdAsync(10))
                .ReturnsAsync(photographer);


            _photographerRepositoryMock
                .Setup(x =>
                    x.GetMyBookingsAsync(5))
                .ReturnsAsync(
                    new List<Booking>());


            // Act

            var result =
                await _service
                    .GetMyBookingsAsyncByUsingProjection(10);


            // Assert

            Assert.NotNull(result);

            var resultList =
                result.ToList();


            Assert.Empty(resultList);


            // Verify repository calls

            _photographerRepositoryMock.Verify(
                x =>
                    x.GetByUserIdAsync(10),
                Times.Once);


            _photographerRepositoryMock.Verify(
                x =>
                    x.GetMyBookingsAsync(5),
                Times.Once);
        }
    }
}