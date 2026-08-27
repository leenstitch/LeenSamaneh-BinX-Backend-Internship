using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class FamilyMedicalHistory
    {
        [Key]
        public int FamilyHistoryId { get; set; }

        public int PatientId { get; set; }

        public string Relationship { get; set; } = string.Empty;

        public string Condition { get; set; } = string.Empty;

        public int? AgeAtDiagnosis { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Property

        public Patient Patient { get; set; } = null!;
    }
}
