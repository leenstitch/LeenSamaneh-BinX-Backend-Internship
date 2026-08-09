//this interface defines the contract for an authentication service that handles user registration.
using APIProject.Dto_s.RegisterDto_s;
using Microsoft.AspNetCore.Identity;

namespace APIProject.Interfaces.InterfacesWeek4
{
    public interface IAuthService
{
        // Registers a new user asynchronously using the provided registration data (RegisterDto).
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
    }
}
