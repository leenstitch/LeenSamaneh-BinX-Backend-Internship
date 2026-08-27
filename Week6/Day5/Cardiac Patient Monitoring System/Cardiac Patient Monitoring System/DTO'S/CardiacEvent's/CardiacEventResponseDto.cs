namespace Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s
{
    public class CardiacEventResponseDto
    {
        public int CardiacEventId { get; set; }

        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Outcome { get; set; }

        public string? Notes { get; set; }
    }
}