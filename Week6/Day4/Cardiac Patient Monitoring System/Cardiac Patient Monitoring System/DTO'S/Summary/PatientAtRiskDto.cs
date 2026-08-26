namespace Cardiac_Patient_Monitoring_System.DTO_S.AdminDashboardDto_s
{
    public class PatientAtRiskDto
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public List<string> Alerts { get; set; }
            = new List<string>();
    }
}