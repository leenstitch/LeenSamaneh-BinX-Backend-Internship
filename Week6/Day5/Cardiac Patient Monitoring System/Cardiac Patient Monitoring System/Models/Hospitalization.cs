using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Hospitalization
    {
        [Key]
        public int HospitalizationId { get; set; }

        public int PatientId { get; set; }

        public string HospitalName { get; set; } = string.Empty;

        public DateTime AdmissionDate { get; set; }

        public DateTime? DischargeDate { get; set; }

        public string? Reason { get; set; }

        public string? Diagnosis { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Property

        public Patient Patient { get; set; } = null!;
    }
}
