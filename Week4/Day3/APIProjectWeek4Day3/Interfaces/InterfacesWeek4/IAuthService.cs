//this interface defines the contract for an authentication service that handles user registration.
using APIProject.Dto_s.Week4Dto_s.LoginDto_s;
using APIProject.Dto_s.Week4Dto_s.RegisterDto_s;
using APIProjectWeek4Day2.Dto_s.Week4Dto_s.LoginDto_s;
using Microsoft.AspNetCore.Identity;

namespace APIProject.Interfaces.InterfacesWeek4
{
    public interface IAuthService
{
        // Registers a new user asynchronously using the provided registration data (RegisterDto).
        Task<IdentityResult> RegisterAsync(RegisterDto dto);

        // Logs in a user asynchronously using the provided login data (LoginDto).
        Task<TokenResponseDto?> LoginAsync(LoginDto dto);
    }
}
