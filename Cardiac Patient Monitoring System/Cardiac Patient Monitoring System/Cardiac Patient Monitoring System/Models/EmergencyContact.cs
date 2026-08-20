using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class EmergencyContact
    {
        [Key]
        public int EmergencyContactId { get; set; }

        public int PatientId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Relation { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public string? Email { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Property
        public Patient Patient { get; set; } = null!;
    }
}
