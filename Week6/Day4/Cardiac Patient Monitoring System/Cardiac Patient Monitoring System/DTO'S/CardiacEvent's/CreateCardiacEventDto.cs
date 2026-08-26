using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s
{
    public class CreateCardiacEventDto
    {
        //[Required]
       // public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        [Required]
        public string EventType { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Outcome { get; set; }

        public string? Notes { get; set; }
    }
}