using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s
{
    public class CreateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }

        [Required]
        [MinLength(3)]
        public string Reason { get; set; } = string.Empty;
        [Required]
        [MinLength(2)]
        public string Location { get; set; } = string.Empty;

        public string? Notes { get; set; }
        public string? RecordedByDoctorName { get; set; }
    }
}
