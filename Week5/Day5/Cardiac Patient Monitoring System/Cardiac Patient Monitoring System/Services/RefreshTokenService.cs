using System.Security.Cryptography;
using Cardiac_Patient_Monitoring_System.Configuration;
using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationDbContext _context;

        private readonly IJwtService _jwtService;

        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(
            ApplicationDbContext context,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var randomNumberGenerator =
                RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(
            string refreshToken)
        {
            var storedToken =
                await _context.RefreshTokens
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(
                        x => x.Token == refreshToken);

            if (storedToken == null)
                return null;

            if (storedToken.ExpiresAt <= DateTime.UtcNow)
                return null;

            if (storedToken.IsRevoked)
                return null;

            var accessToken =
                await _jwtService.CreateToken(
                    storedToken.User);

            var newRefreshToken =
                GenerateRefreshToken();

            storedToken.IsRevoked = true;

            var newRefreshTokenEntity =
                new RefreshToken
                {
                    Token = newRefreshToken,
                    UserId = storedToken.UserId,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(
                        _jwtSettings.RefreshTokenExpirationDays),
                    IsRevoked = false
                };

            _context.RefreshTokens.Add(
                newRefreshTokenEntity);

            await _context.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}

