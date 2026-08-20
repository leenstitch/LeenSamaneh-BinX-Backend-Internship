using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class VitalSign
    {
        [Key]
        public int VitalSignId { get; set; }

        public int PatientId { get; set; }

        public string? RecordedByDoctorName { get; set; }

        public int HeartRate { get; set; }

        public int SystolicPressure { get; set; }

        public int DiastolicPressure { get; set; }

        public decimal OxygenSaturation { get; set; }

        public decimal Temperature { get; set; }

        public DateTime MeasuredAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Notes { get; set; }


        // Navigation Properties
        public Patient Patient { get; set; } = null!;

        
    }
}
