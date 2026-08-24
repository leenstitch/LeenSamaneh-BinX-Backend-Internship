using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Allergy
    {
        [Key]
        public int AllergyId { get; set; }

        public int PatientId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Reaction { get; set; }

        public string? Severity { get; set; }

        public DateTime? DiagnosedAt { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Property

        public Patient Patient { get; set; } = null!;
    }
}
