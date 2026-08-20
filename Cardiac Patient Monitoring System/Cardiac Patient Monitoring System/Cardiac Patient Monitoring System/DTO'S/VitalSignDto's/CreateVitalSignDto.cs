using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s
{
    public class CreateVitalSignDto
    {
        

        public string? RecordedByDoctorName { get; set; }

        [Range(1, 300)]
        public int ?HeartRate { get; set; }

        [Range(50, 300)]
        public int ?SystolicPressure { get; set; }

        [Range(20, 200)]
        public int? DiastolicPressure { get; set; }

        [Range(0, 100)]
        public decimal? OxygenSaturation { get; set; }

        [Range(25, 45)]
        public decimal? Temperature { get; set; }

        public DateTime ?MeasuredAt { get; set; }

        public string? Notes { get; set; }
    }
}
