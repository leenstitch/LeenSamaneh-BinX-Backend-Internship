using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s
{
    public class CreateDiagnosisDto
    {
        [Required]
        [MinLength(2)]
        public string DiagnosisName { get; set; } = string.Empty;


        [Required]
        public DateTime DiagnosedAt { get; set; }

       // public string? RecordedByDoctorName { get; set; }

        public string? Notes { get; set; }
        public string? DiagnosedByName { get; set; }
        public string? DiagnosedBySpecialization { get; set; }
    }
}
