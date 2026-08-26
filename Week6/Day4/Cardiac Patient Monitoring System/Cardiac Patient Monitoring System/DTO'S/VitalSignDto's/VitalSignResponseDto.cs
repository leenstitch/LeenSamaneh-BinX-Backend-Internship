// Defines the data returned by the API to the client.
namespace Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s
{
    public class VitalSignResponseDto
    {
        public int VitalSignId { get; set; }

        public int PatientId { get; set; }

        public string? RecordedByDoctorName { get; set; }

        public int ?HeartRate { get; set; }

        public int? SystolicPressure { get; set; }

        public int? DiastolicPressure { get; set; }

        public decimal? OxygenSaturation { get; set; }

        public decimal ?Temperature { get; set; }

        public DateTime ?MeasuredAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Notes { get; set; }
    }
}
