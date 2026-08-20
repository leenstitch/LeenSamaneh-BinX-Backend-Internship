// This interface defines the operations provided by the Refresh Token service.
// It handles generating refresh tokens and refreshing expired access tokens.

using Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IRefreshTokenService
    {
        // Generates a new refresh token.
        string GenerateRefreshToken();

        // Validates a refresh token and generates new access
        // and refresh tokens when it is valid.
        Task<TokenResponseDto?> RefreshTokenAsync(
            string refreshToken);
    }
}