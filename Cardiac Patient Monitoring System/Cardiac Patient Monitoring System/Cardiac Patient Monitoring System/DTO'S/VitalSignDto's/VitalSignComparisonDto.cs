namespace Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s
{
    public class VitalSignComparisonDto
    {
        public VitalSignValuesDto Previous { get; set; } = null!;

        public VitalSignValuesDto Latest { get; set; } = null!;

        public VitalSignComparisonValuesDto Comparison { get; set; } = null!;
    }

    public class VitalSignValuesDto
    {
        public int HeartRate { get; set; }

        public int SystolicPressure { get; set; }

        public int DiastolicPressure { get; set; }

        public decimal OxygenSaturation { get; set; }

        public decimal Temperature { get; set; }

        public DateTime MeasuredAt { get; set; }
    }

    public class VitalSignComparisonValuesDto
    {
        public VitalSignMetricComparisonDto HeartRate { get; set; } = null!;

        public VitalSignMetricComparisonDto SystolicPressure { get; set; } = null!;

        public VitalSignMetricComparisonDto DiastolicPressure { get; set; } = null!;

        public VitalSignMetricComparisonDto OxygenSaturation { get; set; } = null!;

        public VitalSignMetricComparisonDto Temperature { get; set; } = null!;
    }

    public class VitalSignMetricComparisonDto
    {
        public decimal Change { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}