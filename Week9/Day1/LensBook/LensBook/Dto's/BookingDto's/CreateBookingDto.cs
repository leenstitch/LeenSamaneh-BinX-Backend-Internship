using System.ComponentModel.DataAnnotations;

namespace LensBook.Dto_s.BookingDto_s
{
    public class CreateBookingDto
    {
        [Required]
        public int PhotographerId { get; set; }

        [Required]
        public int SessionTypeId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public string? Notes { get; set; }
    }
}
