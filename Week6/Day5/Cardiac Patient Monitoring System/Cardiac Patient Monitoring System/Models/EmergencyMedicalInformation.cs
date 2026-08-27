using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class EmergencyMedicalInformation
    {
        [Key]
        public int EmergencyMedicalInformationId { get; set; }

        public int PatientId { get; set; }

        public string? BloodType { get; set; }

        public string? PreferredHospital { get; set; }

        public string? SpecialInstructions { get; set; }

        public string? EmergencyNotes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Property

        public Patient Patient { get; set; } = null!;
    }
}
