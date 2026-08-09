// This controller handles user authentication-related requests. 
// It provides an endpoint for registering a new user.
// The registration data is received through RegisterDto and passed to the Auth
//Related files:
/*
 IAuthService.cs
 AuthService.cs
 */
using APIProject.Dto_s.RegisterDto_s;
using APIProject.Interfaces.InterfacesWeek4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Week4Controller.Auth
{
    [Route("api/v1/accounts")]
    [ApiController]
public class AuthController : ControllerBase
{
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Handles POST requests for registering a new user.
        // post : api/v1/accounts/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Sends the registration data to the Service Layer.
            var result = await _authService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                message = "User registered successfully"
            });
        }
    }
}
