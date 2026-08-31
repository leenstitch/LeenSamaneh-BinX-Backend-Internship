using LensBook.Dto_s.Auth;
using LensBook.Dto_s.RegisterCustomerDto_s;


namespace LensBook.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(
            RegisterCustomerDto dto);

        Task<AuthResponseDto> LoginAsync(
            LoginDto dto);

        Task<AuthResponseDto> RefreshTokenAsync(
            string refreshToken);
    }
}