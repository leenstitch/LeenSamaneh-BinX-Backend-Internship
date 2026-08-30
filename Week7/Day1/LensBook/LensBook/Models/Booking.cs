using System.ComponentModel.DataAnnotations;

namespace LensBook.Models
{
    public class Booking
    {
        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Completed,
            Cancelled
        }

        [Key]
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public int PhotographerId { get; set; }

        public int SessionTypeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Properties

        public Customer Customer { get; set; } = null!;

        public Photographer Photographer { get; set; } = null!;

        public SessionType SessionType { get; set; } = null!;
    }
}
