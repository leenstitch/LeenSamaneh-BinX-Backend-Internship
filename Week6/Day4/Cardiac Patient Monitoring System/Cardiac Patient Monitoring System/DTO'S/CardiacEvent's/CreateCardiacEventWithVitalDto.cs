namespace Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s
{
    public class CreateCardiacEventWithVitalDto
    {
        public int? DoctorId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Outcome { get; set; }

        public string? Notes { get; set; }

        public int HeartRate { get; set; }

        public int SystolicPressure { get; set; }

        public int DiastolicPressure { get; set; }

        public decimal OxygenSaturation { get; set; }

        public decimal Temperature { get; set; }

        public string? VitalNotes { get; set; }
    }
}
