using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LensBook.Configuration;

using LensBook.DATA;
using LensBook.Dto_s.Auth;
using LensBook.Dto_s.RegisterCustomerDto_s;

using LensBook.Models;

using LensBook.Repository_s.IRepository;
using LensBook.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LensBook.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser>
            _userManager;

        private readonly IAuthRepository
            _authRepository;

        private readonly ApplicationDbContext
            _context;

        private readonly JwtSettings
            _jwtSettings;


        public AuthService(
            UserManager<ApplicationUser> userManager,
            IAuthRepository authRepository,
            ApplicationDbContext context,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _authRepository = authRepository;
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }


        // =====================================================
        // LOGIN
        // =====================================================

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto dto)
        {
            // -------------------------------------------------
            // 1. Find user by email
            // -------------------------------------------------

            var user =
                await _userManager.FindByEmailAsync(
                    dto.Email);

            if (user == null)
            {
                throw new Exception(
                    "Invalid email or password.");
            }


            // -------------------------------------------------
            // 2. Check password
            // -------------------------------------------------

            var passwordValid =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password);

            if (!passwordValid)
            {
                throw new Exception(
                    "Invalid email or password.");
            }


            // -------------------------------------------------
            // 3. Get user's roles
            // -------------------------------------------------

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                throw new Exception(
                    "User does not have a role.");
            }


            // -------------------------------------------------
            // 4. Generate Access Token
            // -------------------------------------------------

            var accessToken =
                await GenerateAccessTokenAsync(user);


            // -------------------------------------------------
            // 5. Generate Refresh Token
            // -------------------------------------------------

            var refreshToken =
                GenerateRefreshToken(user.Id);


            // -------------------------------------------------
            // 6. Save Refresh Token
            // -------------------------------------------------

            await _authRepository
                .AddRefreshTokenAsync(refreshToken);

            await _authRepository
                .SaveChangesAsync();


            // -------------------------------------------------
            // 7. Return tokens
            // -------------------------------------------------

            return new AuthResponseDto
            {
                AccessToken = accessToken,

                RefreshToken =
                    refreshToken.Token,

                AccessTokenExpiration =
                    DateTime.UtcNow.AddMinutes(
                        _jwtSettings
                            .AccessTokenExpirationMinutes),

                RefreshTokenExpiration =
                    refreshToken.ExpiresAt
            };
        }


        // =====================================================
        // GENERATE ACCESS TOKEN
        // =====================================================

        private async Task<string>
            GenerateAccessTokenAsync(
                ApplicationUser user)
        {
            // -------------------------------------------------
            // 1. Get roles
            // -------------------------------------------------

            var roles =
                await _userManager.GetRolesAsync(user);


            // -------------------------------------------------
            // 2. Create basic claims
            // -------------------------------------------------

            var claims =
                new List<Claim>
                {
                    new Claim(
                        JwtRegisteredClaimNames.Sub,
                        user.Id.ToString()),

                    new Claim(
                        JwtRegisteredClaimNames.Email,
                        user.Email ?? string.Empty),

                    new Claim(
                        ClaimTypes.NameIdentifier,
                        user.Id.ToString())
                };


            // -------------------------------------------------
            // 3. Add roles to JWT
            // -------------------------------------------------

            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }


            // -------------------------------------------------
            // 4. Customer-specific claim
            // -------------------------------------------------

            if (roles.Contains("Customer"))
            {
                var customer =
                    await _authRepository
                        .GetCustomerByUserIdAsync(
                            user.Id);

                if (customer != null)
                {
                    claims.Add(
                        new Claim(
                            "CustomerId",
                            customer.CustomerId
                                .ToString()));
                }
            }


            // -------------------------------------------------
            // 5. Photographer-specific claim
            // -------------------------------------------------

            if (roles.Contains("Photographer"))
            {
                var photographer =
                    await _authRepository
                        .GetPhotographerByUserIdAsync(
                            user.Id);

                if (photographer != null)
                {
                    claims.Add(
                        new Claim(
                            "PhotographerId",
                            photographer
                                .PhotographerId
                                .ToString()));
                }
            }


            // -------------------------------------------------
            // 6. Create signing key
            // -------------------------------------------------

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _jwtSettings.Key));


            // -------------------------------------------------
            // 7. Create credentials
            // -------------------------------------------------

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);


            // -------------------------------------------------
            // 8. Token expiration
            // -------------------------------------------------

            var expiration =
                DateTime.UtcNow.AddMinutes(
                    _jwtSettings
                        .AccessTokenExpirationMinutes);


            // -------------------------------------------------
            // 9. Create JWT
            // -------------------------------------------------

            var token =
                new JwtSecurityToken(
                    issuer:
                        _jwtSettings.Issuer,

                    audience:
                        _jwtSettings.Audience,

                    claims:
                        claims,

                    expires:
                        expiration,

                    signingCredentials:
                        credentials);


            // -------------------------------------------------
            // 10. Convert token to string
            // -------------------------------------------------

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }


        // =====================================================
        // GENERATE REFRESH TOKEN
        // =====================================================

        private RefreshToken GenerateRefreshToken(
            int userId)
        {
            var randomBytes =
                RandomNumberGenerator.GetBytes(64);

            return new RefreshToken
            {
                Token =
                    Convert.ToBase64String(
                        randomBytes),

                UserId = userId,

                CreatedAt =
                    DateTime.UtcNow,

                ExpiresAt =
                    DateTime.UtcNow.AddDays(
                        _jwtSettings
                            .RefreshTokenExpirationDays),

                IsRevoked = false
            };
        }


        // =====================================================
        // REFRESH TOKEN
        // =====================================================

        public async Task<AuthResponseDto>
            RefreshTokenAsync(
                string refreshToken)
        {
            // -------------------------------------------------
            // 1. Find refresh token
            // -------------------------------------------------

            var storedToken =
                await _authRepository
                    .GetRefreshTokenAsync(
                        refreshToken);

            if (storedToken == null)
            {
                throw new Exception(
                    "Invalid refresh token.");
            }


            // -------------------------------------------------
            // 2. Check if revoked
            // -------------------------------------------------

            if (storedToken.IsRevoked)
            {
                throw new Exception(
                    "Refresh token has been revoked.");
            }


            // -------------------------------------------------
            // 3. Check expiration
            // -------------------------------------------------

            if (storedToken.ExpiresAt <=
                DateTime.UtcNow)
            {
                throw new Exception(
                    "Refresh token has expired.");
            }


            // -------------------------------------------------
            // 4. Get user
            // -------------------------------------------------

            var user =
                await _userManager.FindByIdAsync(
                    storedToken.UserId.ToString());

            if (user == null)
            {
                throw new Exception(
                    "User not found.");
            }


            // -------------------------------------------------
            // 5. Get roles
            // -------------------------------------------------

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                throw new Exception(
                    "User does not have a role.");
            }


            // -------------------------------------------------
            // 6. Revoke old refresh token
            // -------------------------------------------------

            await _authRepository
                .RevokeRefreshTokenAsync(
                    storedToken);


            // -------------------------------------------------
            // 7. Generate new Access Token
            // -------------------------------------------------

            var newAccessToken =
                await GenerateAccessTokenAsync(user);


            // -------------------------------------------------
            // 8. Generate new Refresh Token
            // -------------------------------------------------

            var newRefreshToken =
                GenerateRefreshToken(user.Id);


            // -------------------------------------------------
            // 9. Save new Refresh Token
            // -------------------------------------------------

            await _authRepository
                .AddRefreshTokenAsync(
                    newRefreshToken);

            await _authRepository
                .SaveChangesAsync();


            // -------------------------------------------------
            // 10. Return new tokens
            // -------------------------------------------------

            return new AuthResponseDto
            {
                AccessToken =
                    newAccessToken,

                RefreshToken =
                    newRefreshToken.Token,

                AccessTokenExpiration =
                    DateTime.UtcNow.AddMinutes(
                        _jwtSettings
                            .AccessTokenExpirationMinutes),

                RefreshTokenExpiration =
                    newRefreshToken.ExpiresAt
            };
        }


        // =====================================================
        // REGISTER
        // =====================================================

        public async Task<RegisterResponseDto>
     RegisterAsync(
         RegisterCustomerDto dto)
        {
            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                // 1. Create Identity User
                var user =
                    new ApplicationUser
                    {
                        UserName = dto.Email,
                        Email = dto.Email,
                        PhoneNumber = dto.PhoneNumber
                    };

                var result =
                    await _userManager.CreateAsync(
                        user,
                        dto.Password);

                if (!result.Succeeded)
                {
                    var errors =
                        string.Join(
                            ", ",
                            result.Errors.Select(
                                e => e.Description));

                    throw new Exception(
                        $"Failed to create user: {errors}");
                }


                // 2. Add Customer Role
                var roleResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        "Customer");

                if (!roleResult.Succeeded)
                {
                    var errors =
                        string.Join(
                            ", ",
                            roleResult.Errors.Select(
                                e => e.Description));

                    throw new Exception(
                        $"Failed to assign Customer role: {errors}");
                }


                // 3. Create Customer
                var customer =
                    new Customer
                    {
                        UserId = user.Id,

                        FirstName =
                            dto.FirstName,

                        LastName =
                            dto.LastName,

                        PhoneNumber =
                            dto.PhoneNumber,

                        CreatedAt =
                            DateTime.UtcNow,

                        UpdatedAt =
                            DateTime.UtcNow
                    };


                // 4. Add Customer
                await _authRepository
                    .AddCustomerAsync(customer);


                // 5. Save
                await _authRepository
                    .SaveChangesAsync();


                // 6. Commit transaction
                await transaction.CommitAsync();


                // 7. Return success response
                return new RegisterResponseDto
                {
                    Message =
                        "Registration successful."
                };
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        
    }
    }
}