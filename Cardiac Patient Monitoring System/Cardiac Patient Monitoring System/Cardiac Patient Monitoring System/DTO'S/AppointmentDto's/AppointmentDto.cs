namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string? RecordedByDoctorName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}