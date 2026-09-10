namespace LensBook.Dto_s.ExternalScheduleDto_s
{
    public class ExternalScheduleResponseDto
    {
        public int ExternalScheduleId { get; set; }

        public int PhotographerId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Location { get; set; }

        public string? Notes { get; set; }
    }
}
