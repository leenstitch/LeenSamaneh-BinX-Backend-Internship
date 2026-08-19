using Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();

        Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken);
    }
}
