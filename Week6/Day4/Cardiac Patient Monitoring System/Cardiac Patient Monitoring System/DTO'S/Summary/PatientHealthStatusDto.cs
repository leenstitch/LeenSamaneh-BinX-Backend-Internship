namespace Cardiac_Patient_Monitoring_System.DTO_S.Summary
{
    public class PatientHealthStatusDto
    {
        public string Status { get; set; } = string.Empty;

        public List<string> Alerts { get; set; }
            = new List<string>();

        public DateTime? LatestMeasuredAt { get; set; }
    }
}
