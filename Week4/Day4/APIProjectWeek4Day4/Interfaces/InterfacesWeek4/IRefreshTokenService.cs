// This code defines an interface for a refresh token service.

using APIProjectWeek4Day2.Dto_s.Week4Dto_s.LoginDto_s;

namespace APIProject.Interfaces.InterfacesWeek4
{
    public interface IRefreshTokenService
    {
        // This method is responsible for generating a new refresh token.
        string GenerateRefreshToken();

        // This method is responsible for refreshing the access token using the provided refresh token.
        Task<TokenResponseDto?> RefreshTokenAsync(
            string refreshToken);
    }
}
