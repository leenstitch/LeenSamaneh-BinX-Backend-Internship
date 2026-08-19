using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }

        public int PatientId { get; set; }

        public string? PrescribedByDoctorName { get; set; }

        public string? PrescribedBySpecialization { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public string Frequency { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Properties
        public Patient Patient { get; set; } = null!;

        
    }
}
