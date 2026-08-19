using Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s;
using Microsoft.AspNetCore.Identity;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);

        // Logs in a user asynchronously using the provided login data (LoginDto).
        Task<TokenResponseDto?> LoginAsync(LoginDto dto);



    }
}
