namespace Cardiac_Patient_Monitoring_System.DTO_S.MedicalTimelineItemDto_s
{
    public class MedicalTimelineResponseDto
    {
        public int PatientId { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public List<MedicalTimelineItemDto> Items { get; set; } = new();
    }
}
