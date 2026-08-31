using System.ComponentModel.DataAnnotations;

namespace LensBook.Models
{
    public class Photographer
    {
        [Key]
        public int PhotographerId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Properties

        public ApplicationUser User { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; }
            = new List<Booking>();


        public ICollection<ExternalSchedule> ExternalSchedules { get; set; }
            = new List<ExternalSchedule>();
    }
}
