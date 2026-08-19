//
// This test class contains unit tests for the AuthService.
//
// It uses xUnit for writing and running tests and Moq for mocking
// external dependencies such as UserManager, SignInManager, JWT
// service, and Refresh Token service.
//
// The tests cover both successful and failure scenarios for:
//
// ======================= Registration Tests =======================
//
// 1. RegisterAsync_ReturnsSuccess_WhenDataIsValid
//    - Verifies that a new user can register successfully.
//    - Checks that the user is created.
//    - Checks that the Patient role is assigned.
//    - Checks that a Patient profile is created with the correct data.
//
// 2. RegisterAsync_ReturnsFailure_WhenEmailAlreadyExists
//    - Verifies that registration fails when the email is already registered.
//    - Ensures that a new user is not created.
//    - Ensures that no Patient role or Patient profile is created.
//
// 3. RegisterAsync_ReturnsFailure_WhenUserCreationFails
//    - Verifies that registration fails when Identity cannot create the user.
//    - Ensures that role assignment is not attempted.
//    - Ensures that no Patient profile is created.
//
// 4. RegisterAsync_ReturnsFailure_WhenRoleAssignmentFails
//    - Verifies that registration fails when assigning the Patient role fails.
//    - Ensures that the Patient profile is not created.
//    - Tests the failure of a step in the middle of the registration process.
//
// ========================== Login Tests ===========================
//
// 5. LoginAsync_ReturnsNull_WhenUserDoesNotExist
//    - Verifies that login fails when the user does not exist.
//    - Ensures that password verification is not attempted.
//    - Ensures that no JWT token is generated.
//
// 6. LoginAsync_ReturnsNull_WhenPasswordIsInvalid
//    - Verifies that login fails when the password is incorrect.
//    - Ensures that no access token is generated.
//    - Ensures that no refresh token is generated or stored.
//
// 7. LoginAsync_ReturnsTokens_WhenCredentialsAreValid
//    - Verifies successful login with valid credentials.
//    - Checks that an access token is generated.
//    - Checks that a refresh token is generated.
//    - Verifies that the refresh token is stored correctly in the database.
//
// =========================== Coverage =============================
//
// The tests cover:
// - Successful registration and login.
// - Invalid registration scenarios.
// - Duplicate email handling.
// - User creation failures.
// - Role assignment failures.
// - Invalid login credentials.
// - Missing users.
// - JWT and refresh token generation.
// - Database persistence of refresh tokens.
// - Correct interaction with mocked dependencies.
//
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

namespace CardiacPatientMonitoringSystem.Tests.Services
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
            // Supports transactions used by AuthService.
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
            // Other dependencies
            // =====================================================

            _jwtServiceMock =
                new Mock<IJwtService>();

            _refreshTokenServiceMock =
                new Mock<IRefreshTokenService>();

            // =====================================================
            // Create service under test
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
        // REGISTER - SUCCESS
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
        // REGISTER - DUPLICATE EMAIL
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
        // REGISTER - USER CREATION FAILURE
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
        // REGISTER - ROLE FAILURE
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
        // LOGIN - USER NOT FOUND
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
        // LOGIN - INVALID PASSWORD
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
        // LOGIN - SUCCESS
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

            // The RefreshToken has a foreign key to ApplicationUser,
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
        // Helper method
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