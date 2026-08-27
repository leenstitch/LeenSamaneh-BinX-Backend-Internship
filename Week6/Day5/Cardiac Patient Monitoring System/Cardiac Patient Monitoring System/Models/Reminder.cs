using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Reminder
    {
        [Key]
        public int ReminderId { get; set; }

        public int PatientId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int ReminderTypeId { get; set; }

        public DateTime ReminderDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Property

        public Patient Patient { get; set; } = null!;
        public ReminderType ReminderType { get; set; } = null!;
    }
}
