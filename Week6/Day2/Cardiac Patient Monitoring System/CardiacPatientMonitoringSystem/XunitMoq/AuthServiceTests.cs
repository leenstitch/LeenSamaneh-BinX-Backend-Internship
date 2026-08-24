// ================================================================
// AuthService Unit Tests
// ================================================================
//
// This test class contains unit tests for the AuthService.
//
// Testing tools:
// - xUnit: Used to create and run the tests.
// - Moq: Used to mock external dependencies such as UserManager,
//   SignInManager, JWT Service, and Refresh Token Service.
// - SQLite In-Memory: Used as a temporary test database and to
//   support the transaction used during registration.
//
// The tests cover:
//
// Registration:
// 1. Successful registration with valid data.
// 2. Registration failure when the email already exists.
// 3. Registration failure when user creation fails.
// 4. Registration failure when assigning the Patient role fails.
//
// Login:
// 5. Login failure when the user does not exist.
// 6. Login failure when the password is incorrect.
// 7. Successful login with valid credentials.
//
// The tests also verify:
// - Correct interaction with mocked dependencies.
// - Patient profile creation after successful registration.
// - JWT access token generation.
// - Refresh token generation and persistence.
// - Correct handling of success and failure scenarios.
// - That unnecessary operations are not performed when earlier
//   steps fail.
//
// ================================================================
using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CardiacPatientMonitoringSystem.XunitMoq
{
 

    public class AuthServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;

        private readonly Mock<UserManager<ApplicationUser>>
            _userManagerMock;

        private readonly Mock<SignInManager<ApplicationUser>>
            _signInManagerMock;

        private readonly Mock<IJwtService>
            _jwtServiceMock;

        private readonly Mock<IRefreshTokenService>
            _refreshTokenServiceMock;

        private readonly AuthService _service;

        private readonly SqliteConnection _connection;

        public AuthServiceTests()
        {
            // =====================================================
            // SQLite In-Memory Database
            // Supports the transaction used by AuthService.
            // =====================================================

            _connection =
                new SqliteConnection(
                    "Data Source=:memory:");

            _connection.Open();

            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(_connection)
                    .Options;

            _context =
                new ApplicationDbContext(options);

            _context.Database.EnsureCreated();

            // =====================================================
            // UserManager Mock
            // =====================================================

            var userStoreMock =
                new Mock<IUserStore<ApplicationUser>>();

            _userManagerMock =
                new Mock<UserManager<ApplicationUser>>(
                    userStoreMock.Object,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!);

            // =====================================================
            // SignInManager Mock
            // =====================================================

            var httpContextAccessorMock =
                new Mock<IHttpContextAccessor>();

            var userClaimsPrincipalFactoryMock =
                new Mock<
                    IUserClaimsPrincipalFactory<ApplicationUser>>();

            var identityOptions =
                Options.Create(
                    new IdentityOptions());

            var loggerMock =
                new Mock<
                    ILogger<SignInManager<ApplicationUser>>>();

            var authenticationSchemeProviderMock =
                new Mock<
                    IAuthenticationSchemeProvider>();

            var userConfirmationMock =
                new Mock<
                    IUserConfirmation<ApplicationUser>>();

            _signInManagerMock =
                new Mock<
                    SignInManager<ApplicationUser>>(
                    _userManagerMock.Object,
                    httpContextAccessorMock.Object,
                    userClaimsPrincipalFactoryMock.Object,
                    identityOptions,
                    loggerMock.Object,
                    authenticationSchemeProviderMock.Object,
                    userConfirmationMock.Object);

            // =====================================================
            // Other Dependencies
            // =====================================================

            _jwtServiceMock =
                new Mock<IJwtService>();

            _refreshTokenServiceMock =
                new Mock<IRefreshTokenService>();

            // =====================================================
            // Create Service Under Test
            // =====================================================

            _service =
                new AuthService(
                    _userManagerMock.Object,
                    _context,
                    _signInManagerMock.Object,
                    _jwtServiceMock.Object,
                    _refreshTokenServiceMock.Object);
        }

        // =========================================================
        // Test 1: Successful Registration
        // Verifies that a new user can register successfully.
        // Checks user creation, Patient role assignment, and
        // Patient profile creation.
        // =========================================================

        [Fact]
        public async Task RegisterAsync_ReturnsSuccess_WhenDataIsValid()
        {
            // Arrange

            var dto =
                CreateRegisterDto();

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
                    IdentityResult.Success)
                .Callback<ApplicationUser, string>(
                    (user, password) =>
                    {
                        user.Id = 1;

                        _context.Users.Add(user);
                    });

            _userManagerMock
                .Setup(x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Patient"))
                .ReturnsAsync(
                    IdentityResult.Success);

            // Act

            var result =
                await _service.RegisterAsync(dto);

            // Assert

            Assert.True(
                result.Succeeded);

            _userManagerMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<ApplicationUser>(),
                        dto.Password),
                Times.Once);

            _userManagerMock.Verify(
                x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Patient"),
                Times.Once);

            var patient =
                await _context.Patients
                    .FirstOrDefaultAsync();

            Assert.NotNull(patient);

            Assert.Equal(
                dto.FirstName,
                patient.FirstName);

            Assert.Equal(
                dto.LastName,
                patient.LastName);

            Assert.Equal(
                dto.NationalId,
                patient.NationalId);

            Assert.Equal(
                1,
                patient.UserId);
        }

        // =========================================================
        // Test 2: Duplicate Email Registration
        // Verifies that registration fails when the email is already
        // registered and ensures that no new user, role, or patient
        // profile is created.
        // =========================================================

        [Fact]
        public async Task RegisterAsync_ReturnsFailure_WhenEmailAlreadyExists()
        {
            // Arrange

            var dto =
                CreateRegisterDto();

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

            var result =
                await _service.RegisterAsync(dto);

            // Assert

            Assert.False(
                result.Succeeded);

            Assert.Contains(
                result.Errors,
                error =>
                    error.Code ==
                    "DuplicateEmail");

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
                        "Patient"),
                Times.Never);

            Assert.Empty(
                await _context.Patients
                    .ToListAsync());
        }

        // =========================================================
        // Test 3: User Creation Failure
        // Verifies that registration fails when Identity cannot create
        // the user and ensures that role assignment and patient
        // creation are not performed.
        // =========================================================

        [Fact]
        public async Task RegisterAsync_ReturnsFailure_WhenUserCreationFails()
        {
            // Arrange

            var dto =
                CreateRegisterDto();

            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(
                    (ApplicationUser?)null);

            var creationError =
                new IdentityError
                {
                    Code = "PasswordTooWeak",

                    Description =
                        "Password does not meet the requirements."
                };

            _userManagerMock
                .Setup(x =>
                    x.CreateAsync(
                        It.IsAny<ApplicationUser>(),
                        dto.Password))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        creationError));

            // Act

            var result =
                await _service.RegisterAsync(dto);

            // Assert

            Assert.False(
                result.Succeeded);

            Assert.Contains(
                result.Errors,
                error =>
                    error.Code ==
                    "PasswordTooWeak");

            _userManagerMock.Verify(
                x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Patient"),
                Times.Never);

            Assert.Empty(
                await _context.Patients
                    .ToListAsync());
        }

        // =========================================================
        // Test 4: Role Assignment Failure
        // Verifies that registration fails when the Patient role
        // cannot be assigned and ensures that the Patient profile
        // is not created.
        // =========================================================

        [Fact]
        public async Task RegisterAsync_ReturnsFailure_WhenRoleAssignmentFails()
        {
            // Arrange

            var dto =
                CreateRegisterDto();

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
                    IdentityResult.Success)
                .Callback<ApplicationUser, string>(
                    (user, password) =>
                    {
                        user.Id = 1;

                        _context.Users.Add(user);
                    });

            var roleError =
                new IdentityError
                {
                    Code = "RoleError",

                    Description =
                        "Patient role could not be assigned."
                };

            _userManagerMock
                .Setup(x =>
                    x.AddToRoleAsync(
                        It.IsAny<ApplicationUser>(),
                        "Patient"))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        roleError));

            // Act

            var result =
                await _service.RegisterAsync(dto);

            // Assert

            Assert.False(
                result.Succeeded);

            Assert.Contains(
                result.Errors,
                error =>
                    error.Code ==
                    "RoleError");

            _userManagerMock.Verify(
                x =>
                    x.CreateAsync(
                        It.IsAny<ApplicationUser>(),
                        dto.Password),
                Times.Once);

            Assert.Empty(
                await _context.Patients
                    .ToListAsync());
        }

        // =========================================================
        // Test 5: Login With Non-Existing User
        // Verifies that login returns null when no user exists with
        // the provided email and ensures that password validation
        // and token generation are not attempted.
        // =========================================================

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange

            var dto =
                new LoginDto
                {
                    Email =
                        "notfound@example.com",

                    Password =
                        "Password@123"
                };

            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(
                    (ApplicationUser?)null);

            // Act

            var result =
                await _service.LoginAsync(dto);

            // Assert

            Assert.Null(
                result);

            _signInManagerMock.Verify(
                x =>
                    x.CheckPasswordSignInAsync(
                        It.IsAny<ApplicationUser>(),
                        It.IsAny<string>(),
                        false),
                Times.Never);

            _jwtServiceMock.Verify(
                x =>
                    x.CreateToken(
                        It.IsAny<ApplicationUser>()),
                Times.Never);
        }

        // =========================================================
        // Test 6: Login With Invalid Password
        // Verifies that login fails when the password is incorrect
        // and ensures that no access token or refresh token is
        // generated.
        // =========================================================

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenPasswordIsInvalid()
        {
            // Arrange

            var dto =
                new LoginDto
                {
                    Email =
                        "test@example.com",

                    Password =
                        "WrongPassword@123"
                };

            var user =
                new ApplicationUser
                {
                    Id = 1,
                    Email = dto.Email,
                    UserName = dto.Email
                };

            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x =>
                    x.CheckPasswordSignInAsync(
                        user,
                        dto.Password,
                        false))
                .ReturnsAsync(
                    SignInResult.Failed);

            // Act

            var result =
                await _service.LoginAsync(dto);

            // Assert

            Assert.Null(
                result);

            _jwtServiceMock.Verify(
                x =>
                    x.CreateToken(
                        It.IsAny<ApplicationUser>()),
                Times.Never);

            _refreshTokenServiceMock.Verify(
                x =>
                    x.GenerateRefreshToken(),
                Times.Never);

            Assert.Empty(
                await _context.RefreshTokens
                    .ToListAsync());
        }

        // =========================================================
        // Test 7: Successful Login
        // Verifies that a user can log in with valid credentials.
        // Checks access token generation, refresh token generation,
        // and refresh token persistence in the database.
        // =========================================================

        [Fact]
        public async Task LoginAsync_ReturnsTokens_WhenCredentialsAreValid()
        {
            // Arrange

            var dto =
                new LoginDto
                {
                    Email =
                        "test@example.com",

                    Password =
                        "Password@123"
                };

            var user =
                new ApplicationUser
                {
                    Id = 1,
                    Email = dto.Email,
                    UserName = dto.Email
                };

            // RefreshToken has a foreign key to ApplicationUser,
            // so the test user must exist in the test database.

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            _userManagerMock
                .Setup(x =>
                    x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x =>
                    x.CheckPasswordSignInAsync(
                        user,
                        dto.Password,
                        false))
                .ReturnsAsync(
                    SignInResult.Success);

            _jwtServiceMock
                .Setup(x =>
                    x.CreateToken(user))
                .ReturnsAsync(
                    "test-access-token");

            _refreshTokenServiceMock
                .Setup(x =>
                    x.GenerateRefreshToken())
                .Returns(
                    "test-refresh-token");

            // Act

            var result =
                await _service.LoginAsync(dto);

            // Assert

            Assert.NotNull(
                result);

            Assert.Equal(
                "test-access-token",
                result.AccessToken);

            Assert.Equal(
                "test-refresh-token",
                result.RefreshToken);

            _jwtServiceMock.Verify(
                x =>
                    x.CreateToken(user),
                Times.Once);

            _refreshTokenServiceMock.Verify(
                x =>
                    x.GenerateRefreshToken(),
                Times.Once);

            var storedRefreshToken =
                await _context.RefreshTokens
                    .FirstOrDefaultAsync();

            Assert.NotNull(
                storedRefreshToken);

            Assert.Equal(
                user.Id,
                storedRefreshToken.UserId);

            Assert.Equal(
                "test-refresh-token",
                storedRefreshToken.Token);

            Assert.False(
                storedRefreshToken.IsRevoked);

            Assert.True(
                storedRefreshToken.ExpiresAt >
                storedRefreshToken.CreatedAt);
        }

        // =========================================================
        // Helper Method
        // =========================================================

        private static RegisterDto CreateRegisterDto()
        {
            return new RegisterDto
            {
                Email =
                    "newuser@example.com",

                Password =
                    "Password@123",

                Name =
                    "New User",

                FirstName =
                    "New",

                LastName =
                    "User",

                DateOfBirth =
                    new DateTime(
                        1998,
                        5,
                        10),

                PatientGender =
                    Patient.Gender.Male,

                PrimaryPhone =
                    "0591111111",

                NationalId =
                    "NEW123456"
            };
        }

        // =========================================================
        // Dispose
        // =========================================================

        public void Dispose()
        {
            _context.Database.EnsureDeleted();

            _context.Dispose();

            _connection.Close();

            _connection.Dispose();
        }
    }
}