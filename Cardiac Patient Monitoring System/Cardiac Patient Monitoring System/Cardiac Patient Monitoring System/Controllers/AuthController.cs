// This controller handles user authentication and token management.
// It provides registration, login, refresh-token, and patient authorization endpoints.

using Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cardiac_Patient_Monitoring_System.Controllers
{
    [ApiController]
    [Route("api/v1/AuthController")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(
            IAuthService authService,
            IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _refreshTokenService = refreshTokenService;
        }

        // Registers a new user and returns a successful registration response.
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return StatusCode(
    StatusCodes.Status201Created,
    new
    {
        message = "Registration successful."
    });
        }

        // Authenticates a user and returns the generated access
        // and refresh tokens when the credentials are valid.
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(result);
        }

        // Validates a refresh token and generates new access
        // and refresh tokens when the token is valid.
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] string refreshToken)
        {
            var result =
                await _refreshTokenService.RefreshTokenAsync(
                    refreshToken);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token."
                });
            }

            return Ok(result);
        }

        // Protected test endpoint that verifies Patient role authorization.
        [Authorize(Roles = "Patient")]
        [HttpGet("patient-test")]
        public IActionResult PatientTest()
        {
            return Ok(new
            {
                message = "You are a Patient."
            });
        }
    }
}