namespace Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s
{
    public class CardiacEventVitalDto
    {
        public int VitalSignId { get; set; }

        public DateTime MeasuredAt { get; set; }

        public int HeartRate { get; set; }

        public int SystolicPressure { get; set; }

        public int DiastolicPressure { get; set; }

        public decimal OxygenSaturation { get; set; }

        public decimal Temperature { get; set; }

        public string? Notes { get; set; }
    }
}
