using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class CardiacEvent
    {
        [Key]
        public int CardiacEventId { get; set; }

        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Outcome { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Properties

        public Patient Patient { get; set; } = null!;

        public Doctor? Doctor { get; set; }
    }
}
