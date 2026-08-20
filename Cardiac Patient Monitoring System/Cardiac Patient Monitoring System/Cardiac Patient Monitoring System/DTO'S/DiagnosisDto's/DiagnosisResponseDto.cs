using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.DTOs
{
    public class DiagnosisResponseDto
    {
        public int DiagnosisId { get; set; }

        public int PatientId { get; set; }

       

        public string? DiagnosedByName { get; set; }

        public string? DiagnosedBySpecialization { get; set; }

        public string DiagnosisName { get; set; } = string.Empty;

        public DateTime DiagnosedAt { get; set; }

        public string? Notes { get; set; }

      

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}