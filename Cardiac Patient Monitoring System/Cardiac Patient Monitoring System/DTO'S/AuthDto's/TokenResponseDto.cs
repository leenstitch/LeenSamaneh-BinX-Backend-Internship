namespace Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}
