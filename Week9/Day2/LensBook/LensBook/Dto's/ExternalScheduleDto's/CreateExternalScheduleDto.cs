namespace LensBook.Dto_s.ExternalScheduleDto_s
{
    public class CreateExternalScheduleDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Location { get; set; }

        public string? Notes { get; set; }
    }
}
