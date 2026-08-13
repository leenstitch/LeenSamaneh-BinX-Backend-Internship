// this dto is used as a response for login request and refresh token request

namespace APIProjectWeek4Day2.Dto_s.Week4Dto_s.LoginDto_s
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}
