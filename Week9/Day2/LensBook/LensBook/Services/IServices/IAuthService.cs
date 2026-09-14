using LensBook.Dto_s.Auth;
using LensBook.Dto_s.RegisterCustomerDto_s;


namespace LensBook.Services.Interfaces
{
    public interface IAuthService
    {

        //register a new customer
        Task<RegisterResponseDto> RegisterAsync(
            RegisterCustomerDto dto);

        //login
        Task<AuthResponseDto> LoginAsync(
            LoginDto dto);

        //refresh token
        Task<AuthResponseDto> RefreshTokenAsync(
            string refreshToken);
    }
}