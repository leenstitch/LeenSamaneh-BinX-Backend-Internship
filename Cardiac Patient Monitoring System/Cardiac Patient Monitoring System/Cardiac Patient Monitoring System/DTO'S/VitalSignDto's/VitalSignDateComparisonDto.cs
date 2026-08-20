namespace Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s
{
    public class VitalSignDateComparisonDto
    {
        public VitalSignValuesDto FirstDate { get; set; } = null!;

        public VitalSignValuesDto SecondDate { get; set; } = null!;

        public VitalSignComparisonValuesDto Comparison { get; set; } = null!;
    }
}