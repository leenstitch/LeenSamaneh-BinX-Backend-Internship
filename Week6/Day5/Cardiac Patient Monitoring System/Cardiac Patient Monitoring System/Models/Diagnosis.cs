using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Diagnosis
    {
        //public enum DiagnosisStatus
        //{
        //    Active,
        //    Resolved,
        //    Chronic
        //}
        [Key]
        public int DiagnosisId { get; set; }

        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        // public string? RecordedByDoctorName { get; set; }

        public string? DiagnosedByName { get; set; }

        public string? DiagnosedBySpecialization { get; set; }

        public string DiagnosisName { get; set; } = string.Empty;

        public DateTime DiagnosedAt { get; set; }

        public string? Notes { get; set; }

        //public DiagnosisStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime? ConditionStartDate { get; set; }

        // Navigation Properties
        public Patient Patient { get; set; } = null!;
        public Doctor? Doctor { get; set; }

    }
}
