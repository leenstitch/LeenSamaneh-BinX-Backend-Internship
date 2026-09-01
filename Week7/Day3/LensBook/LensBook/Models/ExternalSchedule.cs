using System.ComponentModel.DataAnnotations;

namespace LensBook.Models
{
    public class ExternalSchedule
    {
        [Key]
        public int ExternalScheduleId { get; set; }

        public int PhotographerId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Location { get; set; }

        public string? Notes { get; set; }

       


        // Navigation Property

        public Photographer Photographer { get; set; } = null!;

    }
}
