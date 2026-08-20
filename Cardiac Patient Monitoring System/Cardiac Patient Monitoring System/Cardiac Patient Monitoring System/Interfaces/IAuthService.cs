// This interface defines the authentication operations provided by the application.
// It handles user registration and login using ASP.NET Core Identity and JWT tokens.

using Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s;
using Microsoft.AspNetCore.Identity;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IAuthService
    {
        // Registers a new user and returns the Identity operation result.
        Task<IdentityResult> RegisterAsync(RegisterDto dto);

        // Authenticates a user and returns access and refresh tokens
        // when the login credentials are valid.
        Task<TokenResponseDto?> LoginAsync(LoginDto dto);
    }
}