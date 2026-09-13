namespace LensBook.Dto_s.PhotographerDto_s
{
    public class PhotographerDetailsDto
    {
        public int PhotographerId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Bio { get; set; }

        public List<BookingDetailsDto>? Bookings { get; set; }
          

        public List<ExternalScheduleDto>? ExternalSchedules { get; set; }
            
    }

    public class BookingDetailsDto
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public int SessionTypeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }

    public class ExternalScheduleDto
    {
        public int ExternalScheduleId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Location { get; set; }

        public string? Notes { get; set; }
    }
}

