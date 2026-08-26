// This service handles user registration and login.
// It uses ASP.NET Core Identity, JWT authentication, refresh tokens,
// database transactions, and patient profile creation.

using System.Data;
using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IJwtService _jwtService;

        private readonly IRefreshTokenService _refreshTokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        // Registers a new user, assigns the Patient role,
        // and creates the related patient profile.
        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var existingUser =
                    await _userManager.FindByEmailAsync(dto.Email);

                if (existingUser != null)
                {
                    await transaction.RollbackAsync();

                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "DuplicateEmail",
                            Description = "Email is already registered."
                        });
                }

                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email
                };

                var result =
                    await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return result;
                }

                // Assigns the Patient role to newly registered users.
                var roleResult = await _userManager.AddToRoleAsync(user, "Patient");

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return roleResult;
                }

                // Creates the patient profile linked to the new user.
                var patient = new Patient
                {
                    UserId = user.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    DateOfBirth = dto.DateOfBirth,
                    PatientGender = dto.PatientGender,
                    PrimaryPhone = dto.PrimaryPhone,
                    NationalId = dto.NationalId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Patients.Add(patient);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Rolls back all registration changes if an unexpected
                // exception occurs during the registration process.
                await transaction.RollbackAsync();

                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "RegistrationFailed",
                        Description = ex.Message
                    });
            }
        }

        // Authenticates the user and generates an access token
        // and a refresh token after successful login.
        public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
        {
            var user =
                await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return null;

            var result =
                await _signInManager.CheckPasswordSignInAsync(
                    user,
                    dto.Password,
                    lockoutOnFailure: false);

            if (!result.Succeeded)
                return null;

            // Generates the JWT access token.
            var accessToken =
                await _jwtService.CreateToken(user);

            // Generates the refresh token.
            var refreshToken =
                _refreshTokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            // Stores the refresh token in the database.
            _context.RefreshTokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}