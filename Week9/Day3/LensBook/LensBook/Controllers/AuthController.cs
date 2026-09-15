using LensBook.Dto_s.Auth;
using LensBook.Dto_s.RegisterCustomerDto_s;
using LensBook.Examples;
using LensBook.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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



        // REGISTER CUSTOMER

        /// <summary>
        /// Registers a new customer account.
        /// </summary>
        /// <param name="dto">
        /// The customer registration information.
        /// </param>
        /// <response code="200">
        /// Customer registered successfully.
        /// </response>
        /// <response code="400">
        /// The registration data is invalid or the email already exists.
        /// </response>
        [HttpPost("register")]
        [SwaggerRequestExample(
    typeof(RegisterCustomerDto),
    typeof(RegisterCustomerExample))]

        public async Task<IActionResult> Register(
            RegisterCustomerDto dto)
        {
            var result =
                await _authService
                    .RegisterAsync(dto);

            return Ok(result);
        }



        // LOGIN

        /// <summary>
        /// Authenticates a customer and returns authentication tokens.
        /// </summary>
        /// <param name="dto">
        /// The customer login credentials.
        /// </param>
        /// <response code="200">
        /// Login successful and authentication tokens were returned.
        /// </response>
        /// <response code="400">
        /// The login data is invalid or authentication failed.
        /// </response>
        [HttpPost("login")]
        [SwaggerRequestExample(
    typeof(LoginDto),
    typeof(LoginExample))]

        [SwaggerResponseExample(
    StatusCodes.Status200OK,
    typeof(AuthResponseExample))]
        public async Task<IActionResult> Login(
            LoginDto dto)
        {
            var result =
                await _authService
                    .LoginAsync(dto);

            return Ok(result);
        }



        // REFRESH TOKEN
        /// <summary>
        /// Generates new authentication tokens using a refresh token.
        /// </summary>
        /// <param name="dto">
        /// The refresh token used to generate new authentication tokens.
        /// </param>
        /// <response code="200">
        /// New authentication tokens were generated successfully.
        /// </response>
        /// <response code="400">
        /// The refresh token is invalid or expired.
        /// </response>

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