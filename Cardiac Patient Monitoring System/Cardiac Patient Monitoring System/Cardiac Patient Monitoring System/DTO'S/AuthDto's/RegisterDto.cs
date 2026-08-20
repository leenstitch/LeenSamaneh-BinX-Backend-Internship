using System.ComponentModel.DataAnnotations;
using static Cardiac_Patient_Monitoring_System.Models.Patient;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MinLength(2)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender PatientGender { get; set; }
        [Required]
        [Phone]
        public string PrimaryPhone { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        public string NationalId { get; set; } = string.Empty;

    }
}
