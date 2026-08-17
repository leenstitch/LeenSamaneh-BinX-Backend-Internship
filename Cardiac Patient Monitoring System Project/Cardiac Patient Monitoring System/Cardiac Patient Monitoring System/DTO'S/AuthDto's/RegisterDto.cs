using static Cardiac_Patient_Monitoring_System.Models.Patient;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AuthDto_s
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Gender PatientGender { get; set; }

        public string PrimaryPhone { get; set; } = string.Empty;

        public string NationalId { get; set; } = string.Empty;

    }
}
