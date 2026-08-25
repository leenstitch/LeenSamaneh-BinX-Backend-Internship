using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class ReminderType
    {
        [Key]
        public int ReminderTypeId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation Property
        public ICollection<Reminder> Reminders { get; set; }
            = new List<Reminder>();
    }
}
