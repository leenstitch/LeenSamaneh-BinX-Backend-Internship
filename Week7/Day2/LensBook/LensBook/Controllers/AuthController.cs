using LensBook.Dto_s.Auth;
using LensBook.Dto_s.RegisterCustomerDto_s;

using LensBook.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LensBook.Controllers
{
    [ApiController]
    [Route("api/v1/AuthController")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }


        // =====================================================
        // REGISTER CUSTOMER
        // =====================================================

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterCustomerDto dto)
        {
            var result =
                await _authService
                    .RegisterAsync(dto);

            return Ok(result);
        }


        // =====================================================
        // LOGIN
        // =====================================================

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginDto dto)
        {
            var result =
                await _authService
                    .LoginAsync(dto);

            return Ok(result);
        }


        // =====================================================
        // REFRESH TOKEN
        // =====================================================

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            RefreshTokenDto dto)
        {
            var result =
                await _authService
                    .RefreshTokenAsync(
                        dto.RefreshToken);

            return Ok(result);
        }
    }
}