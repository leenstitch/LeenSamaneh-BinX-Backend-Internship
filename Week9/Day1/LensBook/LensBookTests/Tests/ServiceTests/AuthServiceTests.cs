using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LensBook.Configuration;
using LensBook.DATA;
using LensBook.Dto_s.Auth;
using LensBook.Dto_s.RegisterCustomerDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Moq;

namespace LensBookTests.Tests.ServiceTests
{
    public class AuthServiceTests : IDisposable
    {
        private readonly Mock<UserManager<ApplicationUser>>
            _userManagerMock;

        private readonly Mock<IAuthRepository>
            _authRepositoryMock;

        private readonly JwtSettings
            _jwtSettings;

        private readonly AuthService
            _authService;

        private readonly ApplicationDbContext
            _context;

        private readonly SqliteConnection
            _connection;


        public AuthServiceTests()
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
            // Mock UserManager
            // ==================================================

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


            // ==================================================
            // Mock AuthRepository
            // ==================================================

            _authRepositoryMock =
                new Mock<IAuthRepository>();


            // ==================================================
            // JWT Settings
            // ==================================================

            _jwtSettings =
                new JwtSettings
                {
                    SecretKey =
                        "ThisIsAVeryLongSecretKeyForTesting123456789",

                    Issuer =
                        "LensBook",

                    Audience =
                        "LensBookUsers",

                    AccessTokenExpirationMinutes =
                        30,

                    RefreshTokenExpirationDays =
                        7
                };


            // ==================================================
            // Create AuthService
            // ==================================================

            _authService =
                new AuthService(
                    _userManagerMock.Object,
                    _authRepositoryMock.Object,
                    _context,
                    Options.Create(_jwtSettings));
        }


        // ======================================================
        // LOGIN - HAPPY PATH
        // ======================================================

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsAuthResponse()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var user =
                new ApplicationUser
                {
                    Id = 1,

                    UserName =
                        "customer@test.com",

                    Email =
                        "customer@test.com"
                };


            var dto =
                new LoginDto
                {
                    Email =
                        "customer@test.com",

                    Password =
                        "Password123!"
                };


            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);


            _userManagerMock
                .Setup(x =>
                    x.CheckPasswordAsync(
                        user,
                        dto.Password))
                .ReturnsAsync(true);


            _userManagerMock
                .Setup(x =>
                    x.GetRolesAsync(user))
                .ReturnsAsync(
                    new List<string>
                    {
                        "Customer"
                    });


            var customer =
                new Customer
                {
                    CustomerId = 10,

                    UserId =
                        user.Id
                };


            _authRepositoryMock
                .Setup(x =>
                    x.GetCustomerByUserIdAsync(
                        user.Id))
                .ReturnsAsync(customer);


            _authRepositoryMock
                .Setup(x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()))
                .Returns(Task.CompletedTask);


            _authRepositoryMock
                .Setup(x =>
                    x.SaveChangesAsync())
                .Returns(Task.CompletedTask);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var result =
                await _authService.LoginAsync(dto);


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.NotNull(result);

            Assert.NotNull(
                result.AccessToken);

            Assert.NotEmpty(
                result.AccessToken);

            Assert.NotNull(
                result.RefreshToken);

            Assert.NotEmpty(
                result.RefreshToken);


            Assert.True(
                result.AccessTokenExpiration >
                DateTime.UtcNow);


            Assert.True(
                result.RefreshTokenExpiration >
                DateTime.UtcNow);


            // --------------------------------------------------
            // Verify Repository Calls
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.GetCustomerByUserIdAsync(
                        user.Id),
                Times.Once);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.Is<RefreshToken>(
                            token =>
                                token.UserId ==
                                    user.Id &&

                                token.IsRevoked ==
                                    false)),
                Times.Once);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Once);
        }


        // ======================================================
        // LOGIN - USER NOT FOUND
        // ======================================================

        [Fact]
        public async Task LoginAsync_UserNotFound_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var dto =
                new LoginDto
                {
                    Email =
                        "notfound@test.com",

                    Password =
                        "Password123!"
                };


            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(
                        dto.Email))
                .ReturnsAsync(
                    (ApplicationUser?)null);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.LoginAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Invalid email or password.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.FindByEmailAsync(
                        dto.Email),
                Times.Once);


            _userManagerMock.Verify(
                x =>
                    x.CheckPasswordAsync(
                        It.IsAny<ApplicationUser>(),
                        It.IsAny<string>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // LOGIN - WRONG PASSWORD
        // ======================================================

        [Fact]
        public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var user =
                new ApplicationUser
                {
                    Id = 1,

                    UserName =
                        "customer@test.com",

                    Email =
                        "customer@test.com"
                };


            var dto =
                new LoginDto
                {
                    Email =
                        "customer@test.com",

                    Password =
                        "WrongPassword123!"
                };


            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(
                        dto.Email))
                .ReturnsAsync(user);


            _userManagerMock
                .Setup(x =>
                    x.CheckPasswordAsync(
                        user,
                        dto.Password))
                .ReturnsAsync(false);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.LoginAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Invalid email or password.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.FindByEmailAsync(
                        dto.Email),
                Times.Once);


            _userManagerMock.Verify(
                x =>
                    x.CheckPasswordAsync(
                        user,
                        dto.Password),
                Times.Once);


            _userManagerMock.Verify(
                x =>
                    x.GetRolesAsync(user),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // LOGIN - USER HAS NO ROLE
        // ======================================================

        [Fact]
        public async Task LoginAsync_UserHasNoRole_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var user =
                new ApplicationUser
                {
                    Id = 1,

                    UserName =
                        "customer@test.com",

                    Email =
                        "customer@test.com"
                };


            var dto =
                new LoginDto
                {
                    Email =
                        "customer@test.com",

                    Password =
                        "Password123!"
                };


            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(
                        dto.Email))
                .ReturnsAsync(user);


            _userManagerMock
                .Setup(x =>
                    x.CheckPasswordAsync(
                        user,
                        dto.Password))
                .ReturnsAsync(true);


            _userManagerMock
                .Setup(x =>
                    x.GetRolesAsync(user))
                .ReturnsAsync(
                    new List<string>());


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.LoginAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "User does not have a role.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.GetRolesAsync(user),
                Times.Once);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // REFRESH TOKEN - HAPPY PATH
        // ======================================================

        [Fact]
        public async Task RefreshTokenAsync_ValidToken_ReturnsNewAuthResponse()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var oldRefreshToken =
                new RefreshToken
                {
                    Id = 1,

                    Token =
                        "old-refresh-token",

                    UserId = 10,

                    CreatedAt =
                        DateTime.UtcNow.AddDays(-1),

                    ExpiresAt =
                        DateTime.UtcNow.AddDays(5),

                    IsRevoked = false
                };


            var user =
                new ApplicationUser
                {
                    Id = 10,

                    UserName =
                        "customer@test.com",

                    Email =
                        "customer@test.com"
                };


            _authRepositoryMock
                .Setup(x =>
                    x.GetRefreshTokenAsync(
                        "old-refresh-token"))
                .ReturnsAsync(
                    oldRefreshToken);


            _userManagerMock
                .Setup(x =>
                    x.FindByIdAsync("10"))
                .ReturnsAsync(user);


            _userManagerMock
                .Setup(x =>
                    x.GetRolesAsync(user))
                .ReturnsAsync(
                    new List<string>
                    {
                        "Customer"
                    });


            _authRepositoryMock
                .Setup(x =>
                    x.GetCustomerByUserIdAsync(10))
                .ReturnsAsync(
                    new Customer
                    {
                        CustomerId = 100,

                        UserId = 10
                    });


            _authRepositoryMock
                .Setup(x =>
                    x.RevokeRefreshTokenAsync(
                        oldRefreshToken))
                .Returns(Task.CompletedTask);


            _authRepositoryMock
                .Setup(x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()))
                .Returns(Task.CompletedTask);


            _authRepositoryMock
                .Setup(x =>
                    x.SaveChangesAsync())
                .Returns(Task.CompletedTask);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var result =
                await _authService.RefreshTokenAsync(
                    "old-refresh-token");


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.NotNull(result);

            Assert.NotNull(
                result.AccessToken);

            Assert.NotEmpty(
                result.AccessToken);

            Assert.NotNull(
                result.RefreshToken);

            Assert.NotEmpty(
                result.RefreshToken);


            Assert.NotEqual(
                "old-refresh-token",
                result.RefreshToken);


            Assert.True(
                result.AccessTokenExpiration >
                DateTime.UtcNow);


            Assert.True(
                result.RefreshTokenExpiration >
                DateTime.UtcNow);


            // --------------------------------------------------
            // Verify Old Token Was Revoked
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.RevokeRefreshTokenAsync(
                        oldRefreshToken),
                Times.Once);


            // --------------------------------------------------
            // Verify New Refresh Token Was Added
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.Is<RefreshToken>(
                            token =>
                                token.UserId == 10 &&

                                token.IsRevoked == false &&

                                token.Token !=
                                    "old-refresh-token")),
                Times.Once);


            // --------------------------------------------------
            // Verify SaveChanges
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Once);


            // --------------------------------------------------
            // Verify Customer Lookup
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.GetCustomerByUserIdAsync(10),
                Times.Once);
        }


        // ======================================================
        // REFRESH TOKEN - TOKEN NOT FOUND
        // ======================================================

        [Fact]
        public async Task RefreshTokenAsync_TokenNotFound_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            _authRepositoryMock
                .Setup(x =>
                    x.GetRefreshTokenAsync(
                        "invalid-token"))
                .ReturnsAsync(
                    (RefreshToken?)null);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.RefreshTokenAsync(
                            "invalid-token"));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Invalid refresh token.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.FindByIdAsync(
                        It.IsAny<string>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // REFRESH TOKEN - REVOKED TOKEN
        // ======================================================

        [Fact]
        public async Task RefreshTokenAsync_RevokedToken_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var revokedToken =
                new RefreshToken
                {
                    Id = 1,

                    Token =
                        "revoked-token",

                    UserId = 10,

                    CreatedAt =
                        DateTime.UtcNow.AddDays(-1),

                    ExpiresAt =
                        DateTime.UtcNow.AddDays(5),

                    IsRevoked = true
                };


            _authRepositoryMock
                .Setup(x =>
                    x.GetRefreshTokenAsync(
                        "revoked-token"))
                .ReturnsAsync(
                    revokedToken);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.RefreshTokenAsync(
                            "revoked-token"));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Refresh token has been revoked.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.FindByIdAsync(
                        It.IsAny<string>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // REFRESH TOKEN - EXPIRED TOKEN
        // ======================================================

        [Fact]
        public async Task RefreshTokenAsync_ExpiredToken_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var expiredToken =
                new RefreshToken
                {
                    Id = 1,

                    Token =
                        "expired-token",

                    UserId = 10,

                    CreatedAt =
                        DateTime.UtcNow.AddDays(-10),

                    ExpiresAt =
                        DateTime.UtcNow.AddMinutes(-5),

                    IsRevoked = false
                };


            _authRepositoryMock
                .Setup(x =>
                    x.GetRefreshTokenAsync(
                        "expired-token"))
                .ReturnsAsync(
                    expiredToken);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.RefreshTokenAsync(
                            "expired-token"));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Refresh token has expired.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.FindByIdAsync(
                        It.IsAny<string>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // REFRESH TOKEN - USER NOT FOUND
        // ======================================================

        [Fact]
        public async Task RefreshTokenAsync_UserNotFound_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var storedToken =
                new RefreshToken
                {
                    Id = 1,

                    Token =
                        "valid-token",

                    UserId = 10,

                    CreatedAt =
                        DateTime.UtcNow.AddDays(-1),

                    ExpiresAt =
                        DateTime.UtcNow.AddDays(5),

                    IsRevoked = false
                };


            _authRepositoryMock
                .Setup(x =>
                    x.GetRefreshTokenAsync(
                        "valid-token"))
                .ReturnsAsync(
                    storedToken);


            _userManagerMock
                .Setup(x =>
                    x.FindByIdAsync("10"))
                .ReturnsAsync(
                    (ApplicationUser?)null);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.RefreshTokenAsync(
                            "valid-token"));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "User not found.",
                exception.Message);


            _userManagerMock.Verify(
                x =>
                    x.GetRolesAsync(
                        It.IsAny<ApplicationUser>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.RevokeRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // REFRESH TOKEN - USER HAS NO ROLE
        // ======================================================

        [Fact]
        public async Task RefreshTokenAsync_UserHasNoRole_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var storedToken =
                new RefreshToken
                {
                    Id = 1,

                    Token =
                        "no-role-token",

                    UserId = 10,

                    CreatedAt =
                        DateTime.UtcNow.AddDays(-1),

                    ExpiresAt =
                        DateTime.UtcNow.AddDays(5),

                    IsRevoked = false
                };


            var user =
                new ApplicationUser
                {
                    Id = 10,

                    UserName =
                        "customer@test.com",

                    Email =
                        "customer@test.com"
                };


            _authRepositoryMock
                .Setup(x =>
                    x.GetRefreshTokenAsync(
                        "no-role-token"))
                .ReturnsAsync(
                    storedToken);


            _userManagerMock
                .Setup(x =>
                    x.FindByIdAsync("10"))
                .ReturnsAsync(user);


            _userManagerMock
                .Setup(x =>
                    x.GetRolesAsync(user))
                .ReturnsAsync(
                    new List<string>());


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.RefreshTokenAsync(
                            "no-role-token"));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "User does not have a role.",
                exception.Message);


            _authRepositoryMock.Verify(
                x =>
                    x.RevokeRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.AddRefreshTokenAsync(
                        It.IsAny<RefreshToken>()),
                Times.Never);


            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // REGISTER - HAPPY PATH
        // ======================================================

        [Fact]
        public async Task RegisterAsync_ValidData_ReturnsSuccess()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var dto =
                new RegisterCustomerDto
                {
                    Email =
                        "newcustomer@test.com",

                    Password =
                        "Password123!",

                    FirstName =
                        "Leen",

                    LastName =
                        "Samaneh",

                    PhoneNumber =
                        "0599000000"
                };


            _userManagerMock
                .Setup(x =>
                    x.CreateAsync(
                        It.Is<ApplicationUser>(
                            user =>
                                user.Email == dto.Email &&
                                user.UserName == dto.Email &&
                                user.PhoneNumber == dto.PhoneNumber),
                        dto.Password))
                .Callback<ApplicationUser, string>(
                    (user, password) =>
                    {
                        user.Id = 20;
                    })
                .ReturnsAsync(
                    IdentityResult.Success);


            _userManagerMock
                .Setup(x =>
                    x.AddToRoleAsync(
                        It.Is<ApplicationUser>(
                            user =>
                                user.Id == 20),
                        "Customer"))
                .ReturnsAsync(
                    IdentityResult.Success);


            _authRepositoryMock
                .Setup(x =>
                    x.AddCustomerAsync(
                        It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);


            _authRepositoryMock
                .Setup(x =>
                    x.SaveChangesAsync())
                .Returns(Task.CompletedTask);


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var result =
                await _authService.RegisterAsync(dto);


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.NotNull(result);

            Assert.Equal(
                "Registration successful.",
                result.Message);


            // --------------------------------------------------
            // Verify User Creation
            // --------------------------------------------------

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


            // --------------------------------------------------
            // Verify Customer Role
            // --------------------------------------------------

            _userManagerMock.Verify(
                x =>
                    x.AddToRoleAsync(
                        It.Is<ApplicationUser>(
                            user =>
                                user.Id == 20),
                        "Customer"),
                Times.Once);


            // --------------------------------------------------
            // Verify Customer Creation
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.AddCustomerAsync(
                        It.Is<Customer>(
                            customer =>
                                customer.UserId == 20 &&
                                customer.FirstName == dto.FirstName &&
                                customer.LastName == dto.LastName &&
                                customer.PhoneNumber == dto.PhoneNumber)),
                Times.Once);


            // --------------------------------------------------
            // Verify Save
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Once);
        }


        // ======================================================
        // REGISTER - USER CREATION FAILED
        // ======================================================

        [Fact]
        public async Task RegisterAsync_UserCreationFails_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var dto =
                new RegisterCustomerDto
                {
                    Email =
                        "existing@test.com",

                    Password =
                        "Password123!",

                    FirstName =
                        "Leen",

                    LastName =
                        "Samaneh",

                    PhoneNumber =
                        "0599000000"
                };


            var identityError =
                new IdentityError
                {
                    Code =
                        "DuplicateUserName",

                    Description =
                        "Username already exists."
                };


            _userManagerMock
                .Setup(x =>
                    x.CreateAsync(
                        It.IsAny<ApplicationUser>(),
                        dto.Password))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        identityError));


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.RegisterAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Failed to create user: Username already exists.",
                exception.Message);


            // --------------------------------------------------
            // Verify Role Was NOT Added
            // --------------------------------------------------

            _userManagerMock.Verify(
                x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Customer"),
                Times.Never);


            // --------------------------------------------------
            // Verify Customer Was NOT Created
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.AddCustomerAsync(
                        It.IsAny<Customer>()),
                Times.Never);


            // --------------------------------------------------
            // Verify Save Was NOT Called
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
                Times.Never);
        }


        // ======================================================
        // REGISTER - ROLE ASSIGNMENT FAILED
        // ======================================================

        [Fact]
        public async Task RegisterAsync_RoleAssignmentFails_ThrowsUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // Arrange
            // --------------------------------------------------

            var dto =
                new RegisterCustomerDto
                {
                    Email =
                        "newcustomer@test.com",

                    Password =
                        "Password123!",

                    FirstName =
                        "Leen",

                    LastName =
                        "Samaneh",

                    PhoneNumber =
                        "0599000000"
                };


            _userManagerMock
                .Setup(x =>
                    x.CreateAsync(
                        It.IsAny<ApplicationUser>(),
                        dto.Password))
                .Callback<ApplicationUser, string>(
                    (user, password) =>
                    {
                        user.Id = 30;
                    })
                .ReturnsAsync(
                    IdentityResult.Success);


            var identityError =
                new IdentityError
                {
                    Code =
                        "RoleNotFound",

                    Description =
                        "Customer role does not exist."
                };


            _userManagerMock
                .Setup(x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Customer"))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        identityError));


            // --------------------------------------------------
            // Act
            // --------------------------------------------------

            var exception =
                await Assert.ThrowsAsync<
                    UnauthorizedAccessException>(
                    () =>
                        _authService.RegisterAsync(dto));


            // --------------------------------------------------
            // Assert
            // --------------------------------------------------

            Assert.Equal(
                "Failed to assign Customer role: Customer role does not exist.",
                exception.Message);


            // --------------------------------------------------
            // Verify User Was Created
            // --------------------------------------------------
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


            // --------------------------------------------------
            // Verify Role Assignment
            // --------------------------------------------------

            _userManagerMock.Verify(
                x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Customer"),
                Times.Once);


            // --------------------------------------------------
            // Customer Must NOT Be Created
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.AddCustomerAsync(
                        It.IsAny<Customer>()),
                Times.Never);


            // --------------------------------------------------
            // Save Must NOT Be Called
            // --------------------------------------------------

            _authRepositoryMock.Verify(
                x =>
                    x.SaveChangesAsync(),
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