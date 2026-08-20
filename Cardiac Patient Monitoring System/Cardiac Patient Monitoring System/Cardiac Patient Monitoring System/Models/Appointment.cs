using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Appointment
    {
        public enum AppointmentStatus
        {
            Scheduled,
            Completed,
            Cancelled
        }
        [Key]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string? RecordedByDoctorName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Location { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }


        // Navigation Properties
        public Patient Patient { get; set; } = null!;

       // public Doctor Doctor { get; set; } = null!;
    }
}
