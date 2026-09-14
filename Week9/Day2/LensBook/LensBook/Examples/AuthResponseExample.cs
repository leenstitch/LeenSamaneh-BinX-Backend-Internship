using LensBook.Dto_s.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace LensBook.Examples
{
    public class AuthResponseExample : IExamplesProvider<AuthResponseDto>
    {
        public AuthResponseDto GetExamples()
        {
            return new AuthResponseDto
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                RefreshToken = "d8f7a9b2-1234-4567-8901-abcdef123456",
                AccessTokenExpiration = new DateTime(2026, 9, 14, 18, 0, 0),
                RefreshTokenExpiration = new DateTime(2026, 10, 14, 18, 0, 0)
            };
        }
    }
}