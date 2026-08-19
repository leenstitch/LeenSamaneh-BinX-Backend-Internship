using static Cardiac_Patient_Monitoring_System.Models.Patient;

namespace Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s
{
    public class PatientResponseDto
    {
        public int PatientId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string PatientGender { get; set; } = string.Empty;

        public string PrimaryPhone { get; set; } = string.Empty;

        public string NationalId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
