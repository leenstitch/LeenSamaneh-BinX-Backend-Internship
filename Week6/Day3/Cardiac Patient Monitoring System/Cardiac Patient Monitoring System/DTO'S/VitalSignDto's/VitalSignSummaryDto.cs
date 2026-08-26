namespace Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s
{
    public class VitalSignSummaryDto
    {
        public int ReadingCount { get; set; }

        public double? AverageHeartRate { get; set; }

        public int? MinimumHeartRate { get; set; }

        public int? MaximumHeartRate { get; set; }

        public double? AverageSystolicPressure { get; set; }

        public int? MinimumSystolicPressure { get; set; }

        public int? MaximumSystolicPressure { get; set; }

        public double? AverageDiastolicPressure { get; set; }

        public int? MinimumDiastolicPressure { get; set; }

        public int? MaximumDiastolicPressure { get; set; }

        public double? AverageOxygenSaturation { get; set; }

        public decimal? MinimumOxygenSaturation { get; set; }

        public decimal? MaximumOxygenSaturation { get; set; }

        public int AbnormalReadingCount { get; set; }
    }
}
