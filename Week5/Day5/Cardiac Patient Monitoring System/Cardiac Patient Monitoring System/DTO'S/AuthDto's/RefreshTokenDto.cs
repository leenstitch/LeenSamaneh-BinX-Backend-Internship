using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
