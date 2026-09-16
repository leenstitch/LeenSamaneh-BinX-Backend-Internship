using System.ComponentModel.DataAnnotations;

namespace LensBook.Dto_s.BookingDto_s
{
    public class UpdateBookingStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
