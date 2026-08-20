using System.ComponentModel.DataAnnotations;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s
{
    public class UpdateAppointmentStatusDto
    {
        [Required]
        public Appointment.AppointmentStatus Status { get; set; }
    }
}