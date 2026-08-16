// This controller handles user authentication-related requests. 
// It provides an endpoint for registering a new user.
// The registration data is received through RegisterDto and passed to the Auth
//Related files:
/*
 IAuthService.cs
 AuthService.cs
 IRefreshTokenService.cs
 RefreshTokenService.cs
 */
using APIProject.Dto_s.Week4Dto_s.LoginDto_s;
using APIProject.Dto_s.Week4Dto_s.RegisterDto_s;
using APIProject.Interfaces.InterfacesWeek4;
using APIProjectWeek4Day2.Dto_s.Week4Dto_s.LoginDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Controllers.Week4Controller.Auth
{
    [Route("api/v1/accounts")]
    [ApiController]
public class AuthController : ControllerBase
   {
        private readonly IAuthService _authService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(IAuthService authService,
          IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _refreshTokenService = refreshTokenService;
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

        // Handles POST requests for user login.
        // post : api/v1/accounts/login
        [EnableRateLimiting("LoginPolicy")]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var tokens = await _authService.LoginAsync(dto);

            if (tokens == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(new
            {
                message = "Login successful",
                accessToken = tokens.AccessToken,
                refreshToken = tokens.RefreshToken
            });
        }

        // Handles POST requests for refreshing access tokens using a refresh token.
        // post : api/v1/accounts/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(
         RefreshTokenDto dto)
        {
            var tokens =
                await _refreshTokenService.RefreshTokenAsync(
                    dto.RefreshToken);

            if (tokens == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token."
                });
            }

            return Ok(tokens);
        }



        // This endpoint is protected and requires authentication.
        //it used to verify that the user is authenticated and can access protected resources.
        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            return Ok(new
            {
                message = "You are authenticated!"
            });
        }
    }
}
