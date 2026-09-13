using LensBook.Dto_s.SessionType;
using LensBook.Dto_s.SessionTypeDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services;
using Moq;

namespace LensBookTests.Tests.ServiceTests
{
    public class SessionTypeServiceTests
    {
        private readonly Mock<ISessionTypeRepository>
            _sessionTypeRepositoryMock;

        private readonly SessionTypeService _service;


        public SessionTypeServiceTests()
        {
            // =========================================
            // Create Repository Mock
            // =========================================

            _sessionTypeRepositoryMock =
                new Mock<ISessionTypeRepository>();


            // =========================================
            // Create Service
            // =========================================

            _service =
                new SessionTypeService(
                    _sessionTypeRepositoryMock.Object);
        }


        // =====================================================
        // 1. VALID DATA
        // =====================================================

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsSessionTypeResponse()
        {
            // Arrange

            var dto =
                new CreateSessionTypeDto
                {
                    Name = "Wedding Photography",
                    Description = "Full wedding photography session",
                    DurationInMinutes = 120,
                    Price = 500,
                    IsActive = true
                };

            _sessionTypeRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<SessionType>()))
                .Callback(
                    (SessionType sessionType) =>
                    {
                        sessionType.SessionTypeId = 1;
                    })
                .ReturnsAsync(
                    (SessionType sessionType) => sessionType);


            _sessionTypeRepositoryMock
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
                result.SessionTypeId);

            Assert.Equal(
                dto.Name,
                result.Name);

            Assert.Equal(
                dto.Description,
                result.Description);

            Assert.Equal(
                dto.DurationInMinutes,
                result.DurationInMinutes);

            Assert.Equal(
                dto.Price,
                result.Price);

            Assert.Equal(
                dto.IsActive,
                result.IsActive);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.Is<SessionType>(
                            sessionType =>
                                sessionType.Name == dto.Name &&
                                sessionType.Description == dto.Description &&
                                sessionType.DurationInMinutes ==
                                    dto.DurationInMinutes &&
                                sessionType.Price == dto.Price &&
                                sessionType.IsActive == dto.IsActive)),
                Times.Once);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Once);
        }


        // =====================================================
        // 2. DURATION IS ZERO OR NEGATIVE
        // =====================================================

        [Fact]
        public async Task CreateAsync_InvalidDuration_ThrowsArgumentException()
        {
            // Arrange

            var dto =
                new CreateSessionTypeDto
                {
                    Name = "Wedding Photography",
                    Description = "Test",
                    DurationInMinutes = 0,
                    Price = 500,
                    IsActive = true
                };


            // Act

            var exception =
                await Assert.ThrowsAsync<ArgumentException>(
                    () =>
                        _service.CreateAsync(dto));


            // Assert

            Assert.Equal(
                "Duration must be greater than zero.",
                exception.Message);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<SessionType>()),
                Times.Never);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // =====================================================
        // 3. DURATION EXCEEDS 1440 MINUTES
        // =====================================================

        [Fact]
        public async Task CreateAsync_DurationExceedsLimit_ThrowsArgumentException()
        {
            // Arrange

            var dto =
                new CreateSessionTypeDto
                {
                    Name = "Wedding Photography",
                    Description = "Test",
                    DurationInMinutes = 1441,
                    Price = 500,
                    IsActive = true
                };


            // Act

            var exception =
                await Assert.ThrowsAsync<ArgumentException>(
                    () =>
                        _service.CreateAsync(dto));


            // Assert

            Assert.Equal(
                "Duration cannot exceed 1440 minutes.",
                exception.Message);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<SessionType>()),
                Times.Never);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // =====================================================
        // 4. NEGATIVE PRICE
        // =====================================================

        [Fact]
        public async Task CreateAsync_NegativePrice_ThrowsArgumentException()
        {
            // Arrange

            var dto =
                new CreateSessionTypeDto
                {
                    Name = "Wedding Photography",
                    Description = "Test",
                    DurationInMinutes = 120,
                    Price = -1,
                    IsActive = true
                };


            // Act

            var exception =
                await Assert.ThrowsAsync<ArgumentException>(
                    () =>
                        _service.CreateAsync(dto));


            // Assert

            Assert.Equal(
                "Price cannot be negative.",
                exception.Message);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.AddAsync(
                        It.IsAny<SessionType>()),
                Times.Never);


            _sessionTypeRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }
    }
}