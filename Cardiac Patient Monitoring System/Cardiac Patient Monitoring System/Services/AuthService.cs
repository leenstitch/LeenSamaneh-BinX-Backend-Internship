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

                // Every user who registers normally is a Patient.
                var roleResult = await _userManager.AddToRoleAsync(user, "Patient");

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return roleResult;
                }

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
                await transaction.RollbackAsync();

                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "RegistrationFailed",
                        Description = ex.Message
                    });
            }
        }

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

            var accessToken =
                await _jwtService.CreateToken(user);

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
