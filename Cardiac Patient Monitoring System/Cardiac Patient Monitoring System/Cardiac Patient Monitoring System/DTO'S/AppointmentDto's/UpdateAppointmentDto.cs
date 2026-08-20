using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s
{
    public class UpdateAppointmentDto
    {
        public string? RecordedByDoctorName { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public string? Reason { get; set; }

        //public Appointment.AppointmentStatus? Status { get; set; }

        public string? Location { get; set; }

        public string? Notes { get; set; }
    }
}