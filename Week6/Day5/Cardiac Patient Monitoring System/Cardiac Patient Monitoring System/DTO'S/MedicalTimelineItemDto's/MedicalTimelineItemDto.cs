namespace Cardiac_Patient_Monitoring_System.DTO_S.MedicalTimelineItemDto_s
{
    public class MedicalTimelineItemDto
    {
        public string EventType { get; set; } = string.Empty;

        public int RecordId { get; set; }

        public DateTime Date { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
