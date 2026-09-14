using System.ComponentModel.DataAnnotations;

namespace LensBook.Models
{
    public class SessionType
    {
        [Key]
        public int SessionTypeId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DurationInMinutes { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

    


        // Navigation Property

        public ICollection<Booking> Bookings { get; set; }
            = new List<Booking>();
    }
}
