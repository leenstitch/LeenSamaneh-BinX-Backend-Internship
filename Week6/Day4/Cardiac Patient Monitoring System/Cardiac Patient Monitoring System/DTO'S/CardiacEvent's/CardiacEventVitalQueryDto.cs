namespace Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s
{
    public class CardiacEventVitalQueryDto
    {
        public int DaysBefore { get; set; } = 14;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; } = true;

        public int? MinHeartRate { get; set; }

        public int? MaxHeartRate { get; set; }

        public int? MinSystolicPressure { get; set; }

        public int? MaxSystolicPressure { get; set; }

        public decimal? MinOxygenSaturation { get; set; }

        public decimal? MaxOxygenSaturation { get; set; }
    }
}
