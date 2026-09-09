namespace LensBook.Dto_s.BookingDto_s
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public int PhotographerId { get; set; }

        public int SessionTypeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
