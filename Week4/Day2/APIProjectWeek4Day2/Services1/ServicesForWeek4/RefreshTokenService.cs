//this file contains the implementation of the RefreshTokenService class,
//which is responsible for generating and refreshing JWT tokens.
/*
   related files:
   interface: IRefreshTokenService.cs
   model: RefreshToken.cs
  */

using System.Security.Cryptography;
using APIProject.Data;
using APIProject.Interfaces.InterfacesWeek4;
using APIProjectWeek4Day2.Dto_s.Week4Dto_s.LoginDto_s;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Services1.ServicesForWeek4
{
    public class RefreshTokenService : IRefreshTokenService
    {


        private readonly LibraryDbContext _context;

        //  this is used to generate JSON Web Tokens (JWTs) for authentication.
        private readonly IJwtService _jwtService;

        public RefreshTokenService(
            LibraryDbContext context,
            IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // This method generates a new refresh token using a cryptographically secure random number generator.
        public string GenerateRefreshToken()
        {
            // Generate a random byte array of length 64.
            var randomBytes = new byte[64];

            // Create a cryptographically secure random number generator.
            using var randomNumberGenerator =
                RandomNumberGenerator.Create();

            // Fill the byte array with random bytes.
            randomNumberGenerator.GetBytes(randomBytes);

            // Convert the byte array to a Base64 string and return it as the refresh token.
            return Convert.ToBase64String(randomBytes);
        }

        // This method refreshes the access token using the provided refresh token.
        public async Task<TokenResponseDto?> RefreshTokenAsync(
            string refreshToken)
        {
            // Find the refresh token in the database.
            var storedToken =
                await _context.RefreshTokens
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Token == refreshToken);

            // Token does not exist.
            if (storedToken == null)
                return null;

            // Token has expired.
            if (storedToken.ExpiresAt <= DateTime.UtcNow)
                return null;

            // Token was revoked.
            if (storedToken.IsRevoked)
                return null;

            // Create a new Access Token.
            var accessToken =
                _jwtService.CreateToken(storedToken.User);

            // Create a new Refresh Token.
            var newRefreshToken =
                GenerateRefreshToken();

            // Revoke the old Refresh Token.
            storedToken.IsRevoked = true;

            // Create the new Refresh Token record.
            var newRefreshTokenEntity = new Models.RefreshToken
            {
                Token = newRefreshToken,
                UserId = storedToken.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            // Return the new Access Token and Refresh Token in a TokenResponseDto.
            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}

